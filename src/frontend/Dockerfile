# ========================================================================
# Etapa 1 - Define a imagem a ser utilizada no Front
# ========================================================================
FROM node:18-alpine AS build
WORKDIR /app

# ========================================================================
# Etapa 2 - Restore de dependências
# ========================================================================
COPY package*.json ./

# Install dependencias com --legacy-peer-deps para evitar conflitos (FORCE)
RUN npm install --legacy-peer-deps

# ========================================================================
# Etapa 3 - Build da solução
# ========================================================================
COPY . .

RUN npm run build --prod

# Production stage# Build Angular app (gera dist/MBA.Modulo5)

FROM nginx:alpine

# Copy built app from build stage (ajustado para nome real do output)
COPY --from=build /app/dist/MBA.Modulo5 /usr/share/nginx/html

# Copy nginx configuration
COPY nginx.conf /etc/nginx/nginx.conf

# ========================================================================
# Etapa 4 - Deploy
# ========================================================================
EXPOSE 80

# Start nginx
CMD ["nginx", "-g", "daemon off;"] 