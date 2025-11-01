#!/bin/bash

# Script para alterar IP do projeto PIM

if [ -z "$1" ]; then
    echo "Uso: ./change-ip.sh <novo_ip>"
    echo "Exemplo: ./change-ip.sh 192.168.1.100"
    echo "         ./change-ip.sh localhost"
    exit 1
fi

NEW_IP=$1

# Converter localhost para 127.0.0.1 para Docker
if [ "$NEW_IP" = "localhost" ]; then
    DOCKER_IP="127.0.0.1"
    PROXY_URL="http://localhost:9000"
else
    DOCKER_IP=$NEW_IP
    PROXY_URL="http://$NEW_IP:9000"
fi

echo "Alterando IP para: $NEW_IP"

# Atualizar arquivo .env
sed -i "s/HOST_IP=.*/HOST_IP=$DOCKER_IP/" .env
sed -i "s|BACKEND_URL=.*|BACKEND_URL=http://$NEW_IP:5000|" .env
sed -i "s|FRONTEND_URL=.*|FRONTEND_URL=http://$NEW_IP:5028|" .env
sed -i "s|FAQ_URL=.*|FAQ_URL=http://$NEW_IP:5030|" .env
sed -i "s|PROXY_URL=.*|PROXY_URL=$PROXY_URL|" .env
sed -i "s|BOT_IA_URL=.*|BOT_IA_URL=http://$NEW_IP:8001|" .env
sed -i "s/DB_HOST=.*/DB_HOST=$DOCKER_IP/" .env

# Atualizar chat widget no FAQ
sed -i "s|proxyUrl: 'http://.*:9000'|proxyUrl: '$PROXY_URL'|" faq/Views/Shared/_Layout.cshtml

echo "IP atualizado no arquivo .env e chat widget"

# Chamar script start.sh para rebuild e restart
echo "Chamando start.sh para rebuild containers..."
./start.sh
