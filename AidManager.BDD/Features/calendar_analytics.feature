Feature: Calendario y Analíticas
  Para organizar y medir proyectos
  Como Gestor
  Quiero registrar eventos y registrar analíticas del proyecto

  Background:
    Given ya registré un Manager y tengo su TeamRegisterCode
    And registré un TeamMember con ese TeamRegisterCode
    And creo un proyecto válido y guardo su ProjectId

  Scenario: Crear evento
    When creo un evento para el proyecto
    Then la respuesta es 200 OK

  Scenario: Crear analytics
    When creo analytics para el proyecto
    Then la respuesta es 200 OK
