import { ElementRef } from '@angular/core';
import { FormGroup, UntypedFormGroup } from '@angular/forms';

import { Observable, fromEvent, merge } from 'rxjs';
import { DisplayMessage, GenericValidator, ValidationMessages } from 'src/app/utils/generic-form-validation';
import { LocalStorageUtils } from 'src/app/utils/localstorage';

export abstract class FormBaseComponent {

    displayMessage: DisplayMessage = {};
    genericValidator!: GenericValidator;
    validationMessages!: ValidationMessages;
    dateLogged!: Date;

    mudancasNaoSalvas!: boolean;
    isUserAdmin!: boolean;

    constructor() {
        this.isUserAdmin = new LocalStorageUtils().isUserAdmin();
    }

    protected configureMessagesValidation(validationMessages: ValidationMessages) {
        this.genericValidator = new GenericValidator(validationMessages);
    }

    protected configureValidationFormBase(
        formInputElements: ElementRef[],
        formGroup: UntypedFormGroup) {

        let controlBlurs: Observable<any>[] = formInputElements
            .map((formControl: ElementRef) =>
                fromEvent(formControl.nativeElement, 'blur'));

        merge(...controlBlurs).subscribe(() => {
            this.validateForm(formGroup)
        });
    }

    protected validateForm(formGroup: UntypedFormGroup) {
        this.displayMessage = this.genericValidator.processarMensagens(formGroup);
        this.mudancasNaoSalvas = true;
    }

    protected delay(ms: number) {
        return new Promise(resolve => setTimeout(resolve, ms));
    }

    protected handleApiErrors(fail: any): string[] {
        let errors: string[] = [];
        
        if (fail.status == 400 || fail.status == 409) {
            if (fail.error && fail.error.message) {
                errors.push(fail.error.message);
                return errors;
            }

            // Novo contrato: erros em fail.error.data.errors como objetos { errorMessage }
            const dataErrors = fail?.error?.data?.errors;
            if (Array.isArray(dataErrors)) {
                dataErrors.forEach((e: any) => {
                    if (e && typeof e === 'object' && (e.errorMessage || e.message)) {
                        errors.push(String(e.errorMessage ?? e.message));
                    } else if (typeof e === 'string') {
                        errors.push(e);
                    }
                });
            }

            if (fail.error && fail.error.errors) {
                const validationErrorDictionary = fail.error.errors;
                
                // Se errors é um objeto com chaves (ex: { "CPF": ["CPF é obrigatório"] })
                if (typeof validationErrorDictionary === 'object' && !Array.isArray(validationErrorDictionary)) {
                    for (var fieldName in validationErrorDictionary) {
                        if (validationErrorDictionary.hasOwnProperty(fieldName)) {
                            const fieldErrors = validationErrorDictionary[fieldName];
                            if (Array.isArray(fieldErrors)) {
                                fieldErrors.forEach((error: string) => {
                                    errors.push(error);
                                });
                            } else {
                                errors.push(fieldErrors);
                            }
                        }
                    }
                } 
                // Se errors é um array direto
                else if (Array.isArray(validationErrorDictionary)) {
                    validationErrorDictionary.forEach((error: string) => {
                        errors.push(error);
                    });
                }
            }
        } else {
            console.log('Erro da API:', fail);
            errors.push('Algo deu errado.');
        }
        
        return errors;
    }

    protected passwordsMatch(group: FormGroup) {
        const password = group.get('password')?.value;
        const confirmPasswordControl = group.get('confirmPassword');

        if (password !== confirmPasswordControl?.value) {
            confirmPasswordControl?.setErrors({ notMatching: true });
        } else {
            confirmPasswordControl?.setErrors(null);
        }

        return password === confirmPasswordControl?.value ? null : { notMatching: true };
    }

    /**
     * Método genérico para processar falhas de API
     * Deve ser sobrescrito pelos componentes que precisam de tratamento específico
     */
    protected processFail(fail: any, toastr?: any, form?: FormGroup, fieldsToClear?: string[]): void {
        console.log('fail', fail);
        
        // Limpa campos específicos se fornecidos
        if (form && fieldsToClear && fieldsToClear.length > 0) {
            const clearObject: any = {};
            fieldsToClear.forEach(field => {
                clearObject[field] = '';
            });
            form.patchValue(clearObject);
        }
        
        // Trata erros de validação da API
        // 1) Novo contrato: error.data.errors (array de objetos)
        const dataErrors = fail?.error?.data?.errors;
        if (Array.isArray(dataErrors)) {
            dataErrors.forEach((e: any) => {
                const message = typeof e === 'string' ? e : (e?.errorMessage ?? e?.message);
                if (message && toastr) {
                    toastr.error(String(message), 'Erro de Validação');
                }
            });
            return;
        }

        if (fail && fail.error && fail.error.errors) {
            const errors = fail.error.errors;
            
            // Se errors é um objeto com chaves (ex: { "CPF": ["CPF é obrigatório"] })
            if (typeof errors === 'object' && !Array.isArray(errors)) {
                Object.keys(errors).forEach(field => {
                    const fieldErrors = errors[field];
                    if (Array.isArray(fieldErrors)) {
                        fieldErrors.forEach((errorMessage: string) => {
                            if (toastr) {
                                toastr.error(errorMessage, 'Erro de Validação');
                            }
                        });
                    } else {
                        if (toastr) {
                            toastr.error(fieldErrors, 'Erro de Validação');
                        }
                    }
                });
            } 
            // Se errors é um array direto
            else if (Array.isArray(errors)) {
                errors.forEach((errorMessage: string) => {
                    if (toastr) {
                        toastr.error(errorMessage, 'Erro de Validação');
                    }
                });
            }
        } else if (fail && fail.message) {
            // Se há uma mensagem de erro genérica
            if (toastr) {
                toastr.error(fail.message, 'Erro');
            }
        } else {
            // Erro genérico
            if (toastr) {
                toastr.error('Ocorreu um erro. Tente novamente.', 'Erro');
            }
        }
    }
}