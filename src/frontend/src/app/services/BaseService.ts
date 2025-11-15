import { HttpHeaders, HttpErrorResponse } from "@angular/common/http";
import { throwError } from "rxjs";
import { LocalStorageUtils } from "../utils/localstorage";
import { environment } from "src/environments/environment";

export abstract class BaseService {
    protected UrlServiceV1: string = environment.apiUrlv1;
    public LocalStorage = new LocalStorageUtils();

    constructor() { }

    protected getHeaderJson() {
        return {
            headers: new HttpHeaders({
                'Content-Type': 'application/json'
            })
        };
    }

    protected getAuthHeaderJson() {
        return {
            headers: new HttpHeaders({
                'Content-Type': 'application/json',
                'Accept': 'application/json, text/plain, */*',
                'Authorization': `Bearer ${this.LocalStorage.getUserToken()}`
            })
        };
    }

    protected getAuthHeaderOnly() {
        return new HttpHeaders({
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${this.LocalStorage.getUserToken()}`
        });
    }

    protected serviceError(response: Response | any) {
        let customError: string[] = [];
        let errors: string[] = [];
        let customResponse = { error: { errors: errors } }

        if (response instanceof HttpErrorResponse) {
            if (response.statusText === "Unknown Error") {
                customError.push("Ocorreu um erro desconhecido");
                response.error.errors = customError;
            }
        }

		// Tratamento específico para erros de validação (400 Bad Request)
		if (response.status === 400) {
			const apiError = response?.error ?? {};

			// 1) Novo contrato: erros podem vir em error.data.errors (array de objetos)
			const dataErrors = apiError?.data?.errors;
			if (Array.isArray(dataErrors)) {
				dataErrors.forEach((e: any) => {
					if (e && typeof e === 'object' && (e.errorMessage || e.message)) {
						customError.push(String(e.errorMessage ?? e.message));
					} else if (typeof e === 'string') {
						customError.push(e);
					}
				});
			}

			// 2) Contrato tradicional: error.errors pode ser objeto dicionário ou array
			if (apiError && apiError.errors) {
				const validationErrors = apiError.errors;
				if (typeof validationErrors === 'object' && !Array.isArray(validationErrors)) {
					Object.keys(validationErrors).forEach(field => {
						const fieldErrors = validationErrors[field];
						if (Array.isArray(fieldErrors)) {
							fieldErrors.forEach((errorMessage: string) => {
								customError.push(errorMessage);
							});
						} else if (fieldErrors) {
							customError.push(String(fieldErrors));
						}
					});
				} else if (Array.isArray(validationErrors)) {
					validationErrors.forEach((errorMessage: string) => {
						customError.push(errorMessage);
					});
				}
			}

			// 3) Fallback para título
			if (customError.length === 0) {
				const title = apiError?.title ?? apiError?.data?.title;
				if (title) {
					customError.push(String(title));
				} else {
					customError.push("Dados inválidos. Verifique as informações fornecidas.");
				}
			}

			customResponse.error.errors = customError;
			return throwError(() => customResponse);
		}
        else if (response.status === 500) {
            customError.push("Ocorreu um erro no processamento, tente novamente mais tarde ou contate o nosso suporte.");
            customResponse.error.errors = customError;
            return throwError(() => customResponse);
        }
        else if (response.status === 404) {
            customError.push("O recurso solicitado não existe. Entre em contato com o suporte.");
            customResponse.error.errors = customError;
            return throwError(() => customResponse);
        }
        else if (response.status === 403) {
            customError.push("Você não tem autorização para essa ação. Faça login novamente ou contate o nosso suporte.");
            customResponse.error.errors = customError;
            return throwError(() => customResponse);
        }
        else if (response.status === 401) {
            this.LocalStorage.clear();
            window.location.href = '/login';
        }

        return throwError(() => response);
    }

    protected extractData(response: any): any {
        if (response && typeof response === 'object') {
            // Novo contrato: sempre que houver "data", retorná-lo independentemente de outros campos
            if (Object.prototype.hasOwnProperty.call(response, 'data')) {
                return (response as any).data ?? {};
            }

            // Compatibilidade com contratos anteriores que usavam "result"
            if (Object.prototype.hasOwnProperty.call(response, 'result')) {
                return (response as any).result ?? {};
            }

            // Mantém compatibilidade com estrutura antiga baseada em notifications
            if ((response as any).notifications && (response as any).notifications.length > 0) {
                // Sem transformação específica aqui, retorna o próprio objeto
            }
        }

        return response;
    }

    protected formatDate(date: Date): string {
        return date.toISOString().split('T')[0];
    }
}


