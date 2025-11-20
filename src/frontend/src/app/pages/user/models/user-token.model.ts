
export interface UserTokenModel {
    id: string;
    email: string;
    name: string;
    accessToken: string;
}

export interface Claim {
    type: string;
    value: string;
  }
  
  export interface UsuarioToken {
    id: string;
    email: string;
    claims: Claim[];
  }
  
  export interface UserTokenModel {
    accessToken: string;
    refreshToken: string;
    expiresIn: number;
    usuarioToken: UsuarioToken;
  }