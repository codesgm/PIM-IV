# Como Escrever o Trabalho Acadêmico do PIM

## 📝 Estrutura Básica do Trabalho

### CAPA
```
UNIVERSIDADE PAULISTA - UNIP
CURSO DE ANÁLISE E DESENVOLVIMENTO DE SISTEMAS

[NOMES DOS INTEGRANTES]

SISTEMA INTEGRADO PARA GESTÃO DE CHAMADOS E SUPORTE TÉCNICO 
COM APOIO DE INTELIGÊNCIA ARTIFICIAL - MIDTALK

São Paulo
2025
```

### FOLHA DE ROSTO
```
[NOMES DOS INTEGRANTES]

SISTEMA INTEGRADO PARA GESTÃO DE CHAMADOS E SUPORTE TÉCNICO 
COM APOIO DE INTELIGÊNCIA ARTIFICIAL - MIDTALK

Projeto Integrado Multidisciplinar apresentado 
como exigência para aprovação nas disciplinas 
do 4º semestre do Curso de Análise e 
Desenvolvimento de Sistemas da Universidade 
Paulista - UNIP.

Orientador: Prof. [Nome do Professor]

São Paulo
2025
```

### RESUMO (1 página)
```
RESUMO

Este trabalho apresenta o desenvolvimento do MidTalk, um sistema integrado 
para gestão de chamados e suporte técnico com apoio de inteligência artificial. 
O sistema foi desenvolvido utilizando tecnologias como ASP.NET Core, SQL Server 
e Google Gemini API, atendendo aos requisitos das disciplinas de Projeto de 
Sistemas Orientado a Objetos, Programação Orientada a Objetos II, Desenvolvimento 
para Internet e Gerenciamento de Projetos de Software. O objetivo principal foi 
criar uma solução que otimize o atendimento ao usuário através da implementação 
de um chatbot inteligente que responde automaticamente às dúvidas mais comuns 
e escala para técnicos humanos quando necessário. O sistema contempla interfaces 
desktop, web e mobile, garantindo acessibilidade em diferentes plataformas. 
A implementação seguiu as diretrizes da LGPD para proteção de dados pessoais 
e incorporou práticas modernas de desenvolvimento de software. Os resultados 
demonstram uma redução significativa no tempo de resposta inicial e melhoria 
na experiência do usuário.

Palavras-chave: Sistema de Chamados. Inteligência Artificial. Suporte Técnico. 
ASP.NET Core. LGPD.
```

## 📖 Como Escrever Cada Capítulo

### 1. INTRODUÇÃO (3-4 páginas)

#### 1.1 Contextualização
```
As organizações modernas enfrentam crescentes desafios na gestão eficiente 
de solicitações de suporte técnico. Com o aumento do volume de chamados e 
a necessidade de respostas rápidas, torna-se essencial a implementação de 
sistemas automatizados que possam otimizar o atendimento inicial.

A empresa fictícia MidTech, de médio porte, possui um setor de TI responsável 
por atender solicitações internas de suporte técnico. Atualmente, todas as 
requisições são recebidas por e-mail ou telefone, gerando dificuldades no 
controle dos chamados, atrasos e falhas na priorização.
```

#### 1.2 Justificativa
```
A implementação de um sistema integrado de gestão de chamados com apoio de 
inteligência artificial justifica-se pela necessidade de:

- Reduzir o tempo de resposta inicial aos usuários
- Automatizar respostas para dúvidas frequentes
- Melhorar o controle e rastreamento de chamados
- Otimizar a alocação de recursos técnicos
- Garantir conformidade com a LGPD
```

#### 1.3 Objetivos
```
1.3.1 Objetivo Geral
Desenvolver um sistema integrado para gestão de chamados e suporte técnico 
com apoio de inteligência artificial, visando otimizar o atendimento e 
promover autonomia dos usuários.

1.3.2 Objetivos Específicos
- Implementar chatbot inteligente para respostas automatizadas
- Desenvolver interfaces desktop, web e mobile
- Criar sistema de escalação automática para técnicos
- Garantir conformidade com a LGPD
- Aplicar práticas modernas de desenvolvimento de software
```

### 2. FUNDAMENTAÇÃO TEÓRICA (8-10 páginas)

#### Como escrever:
```
2.1 SISTEMAS DE GESTÃO DE CHAMADOS

Segundo Silva (2023), os sistemas de gestão de chamados são ferramentas 
essenciais para organizações que precisam gerenciar solicitações de suporte 
de forma estruturada. Estes sistemas permitem o registro, acompanhamento e 
resolução de incidentes de forma organizada.

De acordo com Santos e Oliveira (2022), as principais funcionalidades de um 
sistema de chamados incluem:
- Registro e categorização de solicitações
- Atribuição automática de técnicos
- Controle de SLA (Service Level Agreement)
- Geração de relatórios gerenciais

[Continue desenvolvendo cada subtópico com citações]
```

### 3. ANÁLISE E PROJETO (12-15 páginas)

#### Como descrever requisitos:
```
3.1 LEVANTAMENTO DE REQUISITOS

3.1.1 Requisitos Funcionais
RF01 - O sistema deve permitir o cadastro de usuários
RF02 - O sistema deve permitir a abertura de chamados
RF03 - O chatbot deve responder automaticamente às dúvidas
RF04 - O sistema deve escalar chamados para técnicos quando necessário
[Continue listando todos os requisitos]

3.1.2 Requisitos Não Funcionais
RNF01 - O sistema deve suportar até 1000 usuários simultâneos
RNF02 - O tempo de resposta deve ser inferior a 3 segundos
RNF03 - O sistema deve estar disponível 99,9% do tempo
[Continue listando]
```

#### Como descrever diagramas:
```
3.2.1 Diagrama de Casos de Uso

A Figura 1 apresenta o diagrama de casos de uso do sistema MidTalk, 
identificando os principais atores e suas interações com o sistema.

[INSERIR FIGURA 1 - Diagrama de Casos de Uso]

Os atores identificados são:
- Usuário: Pessoa que solicita suporte
- Técnico: Profissional que resolve chamados
- Administrador: Responsável pela gestão do sistema
- Sistema IA: Chatbot que fornece respostas automatizadas

Os principais casos de uso incluem:
- Abrir chamado
- Consultar FAQ
- Interagir com chatbot
- Atribuir técnico
- Resolver chamado
```

### 4. IMPLEMENTAÇÃO (15-20 páginas)

#### Como descrever tecnologias:
```
4.1 AMBIENTE DE DESENVOLVIMENTO

O desenvolvimento do sistema MidTalk utilizou as seguintes tecnologias:

- Linguagem de programação: C# (.NET 9.0)
- Framework web: ASP.NET Core MVC
- Banco de dados: Microsoft SQL Server 2022
- API de IA: Google Gemini API
- Containerização: Docker
- Controle de versão: Git

A escolha dessas tecnologias baseou-se nos requisitos do projeto acadêmico 
e na necessidade de criar uma solução robusta e escalável.
```

#### Como descrever funcionalidades:
```
4.2 DESENVOLVIMENTO DA API BACKEND

A API backend foi desenvolvida seguindo os princípios da arquitetura REST, 
organizando os endpoints de forma lógica e intuitiva. A estrutura do projeto 
segue o padrão MVC (Model-View-Controller), separando as responsabilidades 
em camadas distintas.

4.2.1 Controllers
Os controllers são responsáveis por receber as requisições HTTP e coordenar 
as operações necessárias. Foram implementados os seguintes controllers:

- ChatController: Gerencia operações relacionadas aos chamados
- UserController: Controla operações de usuários
- FAQController: Gerencia perguntas frequentes
- ReportController: Gera relatórios gerenciais

[Inserir código exemplo comentado]
```

### 5. TESTES E VALIDAÇÃO (8-10 páginas)

#### Como documentar testes:
```
5.1 ESTRATÉGIA DE TESTES

A validação do sistema MidTalk foi realizada através de uma estratégia 
abrangente de testes, incluindo testes unitários, de integração e de interface.

5.2 TESTES UNITÁRIOS

Os testes unitários foram implementados utilizando o framework xUnit, 
focando na validação das regras de negócio e funcionalidades críticas.

Tabela 1 - Resultados dos Testes Unitários
| Módulo | Testes | Passou | Falhou | Cobertura |
|--------|--------|--------|--------|-----------|
| Services | 45 | 43 | 2 | 95% |
| Controllers | 32 | 32 | 0 | 100% |
| Models | 18 | 18 | 0 | 100% |

[Inserir screenshots dos resultados]
```

### 6. RESULTADOS (6-8 páginas)

#### Como apresentar resultados:
```
6.1 FUNCIONALIDADES IMPLEMENTADAS

O sistema MidTalk foi desenvolvido com sucesso, atendendo a todos os 
requisitos estabelecidos. As principais funcionalidades implementadas incluem:

6.1.1 Sistema Web
- Interface responsiva para abertura e acompanhamento de chamados
- Dashboard administrativo com relatórios em tempo real
- Sistema de notificações automáticas
- Integração com chatbot inteligente

[Inserir screenshots das telas principais]

6.1.2 Chatbot Inteligente
O chatbot desenvolvido com Google Gemini API demonstrou eficácia na 
resolução de dúvidas comuns, apresentando os seguintes resultados:

- Taxa de resolução automática: 78%
- Tempo médio de resposta: 1.2 segundos
- Satisfação do usuário: 4.2/5.0
```

## 📋 Dicas Práticas de Redação

### Linguagem Acadêmica
```
❌ Evitar: "A gente fez um sistema legal"
✅ Usar: "Foi desenvolvido um sistema que atende aos requisitos estabelecidos"

❌ Evitar: "O sistema é muito bom"
✅ Usar: "O sistema apresenta características que contribuem para..."

❌ Evitar: "Fizemos testes e deu certo"
✅ Usar: "Os testes realizados demonstraram a eficácia da solução"
```

### Citações (mínimo 15 referências)
```
Citação direta curta:
Segundo Silva (2023, p. 45), "os sistemas de IA revolucionaram o atendimento".

Citação indireta:
A implementação de chatbots tem se mostrado eficaz na redução de custos 
operacionais (SANTOS, 2022).

Citação de site:
De acordo com Microsoft (2023), o ASP.NET Core oferece alta performance 
para aplicações web.
```

### Figuras e Tabelas
```
Figura 1 - Arquitetura do Sistema MidTalk
[INSERIR IMAGEM]
Fonte: Elaborado pelos autores (2025)

Tabela 1 - Comparativo de Tecnologias
| Tecnologia | Vantagens | Desvantagens |
|------------|-----------|--------------|
| ASP.NET | Performance | Curva aprendizado |
Fonte: Elaborado pelos autores (2025)
```

## ✅ Checklist Final

### Formatação ABNT
- [ ] Fonte Times New Roman 12
- [ ] Espaçamento 1,5 entre linhas
- [ ] Margens: superior/esquerda 3cm, inferior/direita 2cm
- [ ] Numeração de páginas no canto superior direito
- [ ] Sumário automático
- [ ] Referências em ordem alfabética

### Conteúdo Obrigatório
- [ ] Mínimo 15 referências bibliográficas
- [ ] Código fonte nos apêndices
- [ ] Screenshots de todas as interfaces
- [ ] Diagramas UML explicados
- [ ] Evidências de funcionamento
- [ ] Manual do usuário

### Revisão
- [ ] Ortografia e gramática
- [ ] Coerência entre seções
- [ ] Numeração de figuras/tabelas
- [ ] Citações corretas
- [ ] Conclusão alinhada com objetivos

---

**Dica Principal**: Escreva como se estivesse explicando o projeto para alguém que não conhece. Use linguagem técnica, mas clara e objetiva. Cada afirmação deve ter base teórica (citação) ou evidência prática (screenshot/código).
