Feature: Proyectos y Tareas
  Para gestionar trabajo
  Como Manager
  Quiero crear proyectos y tareas, listarlas, cambiar estado y eliminar

  Background:
    Given ya registré un Manager y tengo su TeamRegisterCode
    And registré un TeamMember con ese TeamRegisterCode

  Scenario: Crear proyecto y tarea, listar, actualizar estado y eliminar
    When creo un proyecto válido
    Then la respuesta es 200 OK
    And guardo el ProjectId
    When creo una tarea ToDo para ese proyecto asignada al TeamMember
    Then la respuesta es 200 OK
    And guardo el TaskId con estado ToDo
    When consulto las tareas del proyecto
    Then la respuesta es 200 OK
    When actualizo la tarea a estado Done
    Then la respuesta es 200 OK
    When elimino la tarea
    Then la respuesta es 200 OK
