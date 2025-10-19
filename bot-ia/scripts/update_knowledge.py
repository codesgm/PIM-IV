#!/usr/bin/env python3
import requests
import os
import logging

logging.basicConfig(level=logging.INFO)
logger = logging.getLogger(__name__)

def fetch_faqs():
    """Busca FAQs do backend"""
    try:
        response = requests.get('http://api:5000/api/faq/export', timeout=10)
        response.raise_for_status()
        return response.json()
    except Exception as e:
        logger.error(f"Erro ao buscar FAQs: {e}")
        return []

def generate_markdown(faqs):
    """Gera markdown das FAQs"""
    markdown = "# Base de Conhecimento MidTalk - FAQs\n\n"
    markdown += "Esta base contém as perguntas frequentes do sistema MidTalk.\n\n"
    
    if not faqs:
        markdown += "Nenhuma FAQ disponível no momento.\n"
        return markdown
    
    # Agrupar por categoria
    categories = {}
    for faq in faqs:
        category = faq.get('categoria', 'Geral')
        if category not in categories:
            categories[category] = []
        categories[category].append(faq)
    
    # Gerar markdown por categoria
    for category, category_faqs in categories.items():
        markdown += f"## {category}\n\n"
        
        for faq in category_faqs:
            question = faq.get('pergunta', 'Pergunta não disponível')
            answer = faq.get('resposta', 'Resposta não disponível')
            
            markdown += f"### {question}\n\n"
            markdown += f"{answer}\n\n"
            markdown += "---\n\n"
    
    return markdown

def update_knowledge_base():
    """Atualiza a base de conhecimento"""
    logger.info("Iniciando atualização da base de conhecimento...")
    
    # Buscar FAQs
    faqs = fetch_faqs()
    logger.info(f"Encontradas {len(faqs)} FAQs")
    
    # Gerar markdown
    markdown = generate_markdown(faqs)
    
    # Salvar arquivo
    knowledge_dir = '/app/knowledge'
    os.makedirs(knowledge_dir, exist_ok=True)
    
    faq_file = os.path.join(knowledge_dir, 'faq_knowledge.md')
    
    try:
        with open(faq_file, 'w', encoding='utf-8') as f:
            f.write(markdown)
        
        logger.info(f"Base de conhecimento atualizada: {faq_file}")
        return True
        
    except Exception as e:
        logger.error(f"Erro ao salvar arquivo: {e}")
        return False

if __name__ == "__main__":
    success = update_knowledge_base()
    exit(0 if success else 1)
