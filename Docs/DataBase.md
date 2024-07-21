# Инструкция по настройке базы данных

Для доступа к базе данных необходимо добавить в фаил с путем
DrugBot\src\DrugBot.ServerApp\appsettings.json
В поля:
<b>"MagicBallConnection"</b>
<b>"ChineseConnection"</b>
<b>"EmojiConnection"</b>
<b>"PredictionConnection"</b>
<b>"WisdomConnection"</b>
добавить строку подключения в БД.

Пример: <b>Host=localhost;Port=5555;Database=my_database;Username=user;Password=password</b>, где:
Host - IP базы данных, localhost равняется 127.0.0.1 - локальный адрес машины,
Port - порт подключения,
Database - название базы данных,
Username - имя пользователя для подключения. Указанный ползователь должен иметь полный доступ
к БД (создание, удаление, изменение и т.д.),
Password - пароль для входа в базу данных под именем указанным в Username.

<b>После успешного подключение к базе данных ее нужно обновить для создания необходимых
для работы таблиц </b>

По умолчанию создается и используется локальная БД SQLite.

