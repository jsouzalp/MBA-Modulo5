# Muito importante! O container não entende CRLF e por isso, se for editar esse arquivo, garanta que o "line feed" esteja no padrão Unix LF
# Para isso, abra o VS Code e siga essa instrução abaixo
# no canto inferior direito, verifique as opções de Encoding (UTF-8) e EndOfLine (CRLF)
# Clique em CRLF e altere para LF
# Salve o arquivo.
# Relembre de sempre executar essa verificação se for mudar algo neste arquivo
/opt/mssql/bin/sqlservr &

echo "Aguardando SQL Server iniciar..."
sleep 20

echo "Executando scripts SQL..."

for file in /scripts/*.sql
do
    if [ -f "$file" ]; then
        echo "Rodando: $file"
        /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P $SA_PASSWORD -C -i "$file"

        if [ $? -ne 0 ]; then
            echo "Erro ao executar $file"
            exit 1
        fi
    fi
done

echo "Todos os scripts foram executados com sucesso!"

wait
