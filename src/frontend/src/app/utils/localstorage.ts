import { environment } from "src/environments/environment";
import { AES, enc } from "crypto-js";
import { UserTokenModel } from "../pages/user/models/user-token.model";

export class LocalStorageUtils {
    private _key = environment.aesKey;


    public getUserToken(): string {
        return localStorage.getItem('mba.grupo1.token') ?? '';
    }

    public setUser(user: UserTokenModel) {
        const envryptedObject = AES.encrypt(JSON.stringify(user), this._key);
        localStorage.setItem('mba.grupo1.user', envryptedObject.toString());
        localStorage.setItem('mba.grupo1.token', user.accessToken);
        localStorage.setItem('mba.grupo1.refreshToken', user.refreshToken);
        localStorage.setItem('mba.grupo1.email', user.usuarioToken.email);
        this.setExpiresAt();
    }

    public getUser(): UserTokenModel {
        const userToken: UserTokenModel = Object.create(null);

        const ttUser = localStorage.getItem('mba.grupo1.user');
        if (!ttUser) return userToken;

        const decrypted2 = AES.decrypt(ttUser!, this._key);
        const decryptedObject = decrypted2.toString(enc.Utf8);
        const user: UserTokenModel = JSON.parse(decryptedObject);
        return user;
    }

    public isUserAdmin(): boolean {
        try {
            const user = this.getUser();
            const claims = user?.usuarioToken?.claims ?? [];
            if (!Array.isArray(claims)) return false;

            return claims.some(c => {
                const type = (c?.type ?? '').toLowerCase();
                const value = c?.value ?? '';
                // Considera formato comum: type = 'role', value = 'Administrador'
                // e o formato solicitado: type = 'Administrador'
                return (type === 'role' && value === 'Administrador') || c?.type === 'Administrador';
            });
        } catch {
            return false;
        }
    }

    private setExpiresAt() {
        let today = new Date();
        var minutesToAdd = 4 * 60;
        let dt = new Date(today.getTime() + minutesToAdd * 60000);
        localStorage.setItem("mba.grupo1.expires_at", JSON.stringify(dt.valueOf()));
    }

    public getExpiresAt(): number {

        const expired = new Date('2000-01-01');
        const milesecExpiration = localStorage.getItem("mba.grupo1.expires_at") ?? expired.valueOf().toString();
        return parseInt(milesecExpiration);
    }

    public getEmail(): string {

        const email = localStorage.getItem('mba.grupo1.email');
        return email ?? '';

    }

    public clear() {
        localStorage.removeItem('mba.grupo1.user');
        localStorage.removeItem('mba.grupo1.token');
        localStorage.removeItem('mba.grupo1.refreshToken');
        localStorage.removeItem("mba.grupo1.expires_at");
    }
}