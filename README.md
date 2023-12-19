# Установщик для системы Matrix
### Состоит из: 
* Web Server - серверная часть для веб интерфейса системы учета и для связи с другими серверами
* Poll Server - сервер для опроса приборов учета
* Sheduler Server - сервер для расписаний опроса
* Check Server - сервер для проверки работоспособности других серверов
* Neo4j - графовая БД для хранения связей
* RabbitMQ - очередь сообщений при общении между серверами и при общении Poll Server с приборами учета
* erlang - зависимость для RabbitMQ
### Самих установочных дистрибутивов в репозитории нет
### Основная логика находится в InstallScenario.cs(управление установкой установочных компонентов) и InstallUnit.cs(установочный компонент)
### InstallPropertiesViewModel.cs - это ViewModel для связки интерфейса и полей в InstallScenario
### ServersConfig.cs - для работы с конфигурационными файлами серверов
