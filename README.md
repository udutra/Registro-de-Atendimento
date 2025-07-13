# Registro de Atendimento - Clínica ACME

A Clínica ACME deseja uma solução web para registrar os atendimentos realizados com seus pacientes. O sistema deve
permitir o **cadastro de pacientes** e o **registro de atendimentos**, com funcionalidades de consulta, edição e
filtros.

---

## ✍️ Funcionalidades Principais

### 1. Cadastro de Pacientes

O cadastro deve conter os seguintes campos:

- **Nome** (obrigatório)
- **Data de nascimento** (obrigatório)
- **CPF** (obrigatório e único)
- **Sexo** (obrigatório)
- **Endereço completo:**
    - CEP
    - Cidade
    - Bairro
    - Endereço
    - Complemento (opcional)
- **Status** (obrigatório – Ativo/Inativo)

#### Requisitos:

- Listar pacientes com filtros por **nome**, **CPF** e **status**;
- Cadastrar e **editar** pacientes;
- **Inativar** pacientes;
- **Validação** para impedir CPF duplicado.

---

### 2. Registro de Atendimentos

Cada atendimento deve conter:

- **Paciente** (obrigatório)
- **Data e hora** (obrigatório – não pode ser no futuro)
- **Descrição do atendimento** (obrigatório, com suporte a textos longos e quebra de linha)
- **Status** (obrigatório – Ativo/Inativo)

#### Requisitos:

- Consultar pacientes, mostrando apenas os que estão **ativos**;
- Cadastrar e **editar** registros de atendimento;
- **Inativar** registros de atendimento;
- Listar atendimentos com filtro por:
    - **Data (período)**
    - **Paciente**
    - **Status**
