Feature: Registro de usuarios (Manager y TeamMember)
  Para habilitar la colaboración
  Como Manager
  Quiero registrar mi cuenta y obtener un TeamRegisterCode, y registrar miembros con ese código

  Scenario: Sign-up Manager genera TeamRegisterCode
    When registro un Manager con datos válidos
    Then la respuesta es 200 OK
    And obtengo un TeamRegisterCode no vacío
    And guardo el CompanyId

  Scenario: Sign-up TeamMember con TeamRegisterCode
    Given ya registré un Manager y tengo su TeamRegisterCode
    When registro un TeamMember usando ese TeamRegisterCode
    Then la respuesta es 200 OK
    And el TeamMember tiene un Id asignado
