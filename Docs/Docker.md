
## How to start

- Первое и очевидное это скачать докер.
- После чего необходимо запустить докер файл в корне проекта
  - `docker build --tag drugbot`
  - запуск в корне необходим ибо файл докера находится там и называется DockerFile
  - У этого есть альтернатива, запуск через конфигурацию райдера Dockerfile
- В итоге будет собран образ `drugbot`
- Теперь необходимо его запустить
```shell
PORT_FOR_WEB=42843
CONTAINER_NAME=drugbot
VK_APP_ID=
VK_GROUP_TOKEN=
PATH_TO_DB=C:\Users\Argon\DrugBotDb

docker run \
    -p $PORT_FOR_WEB:8080 \
    -d \
	--network=bridge \
    --restart=always \
    --name $CONTAINER_NAME \
    -e VK_APP_ID=$VK_APP_ID \
    -e VK_GROUP_TOKEN=$VK_GROUP_TOKEN \
    -v $PATH_TO_DB:/app/Data
    $IMAGE_NAME
```
  - `-p $PORT_FOR_WEB`
    - биндинг порта ко внутреннему порту 8080 
    - порт который будет торчать наружу и через который можно зайти на сайт управления ботами
  - `-d` запуск в фоне
  - `--network=bridge`
    - тип сети, bridge работает внутри и ходит только через определенные порты
    - host, работает как в системе и там нельзя переопределять порты
  - `--restart=always`
    - опция запускающая контейнер если он упал
  - `VK_APP_ID` и `VK_GROUP_TOKEN` переменные окружения, которые необходимо добавить для работы с vk
    - есть альтернативный вариант указать `--env-file ./env`
      - это загрузит в контейнер переменные среды из файла `./env`
      - переменные в файле должны быть в формате `VAR=VAL` и комментариями начинающихся с `#`
  - `-v $PATH_TO_DB` это путь до в системе, где будет сохранена база данных, поскольку хранение в контейнере черевато удалением данных
  - `$IMAGE_NAME` имя собранного образа, в нашем случае `drugbot`
- В случае если вы хотите переопределить настройки приложения, вы можете указать в переменные среды новые значения
  - пример json'а `{"ConnectionStrings": { "DefaultConnection": "DataSource=Data/app.db;Cache=Shared" }}`
  - который будет в перменной `ConnectionStrings:DefaultConnection=DataSource=Data/app.db;Cache=Shared`

## Возможные проблемы и их исправления

### Readonly db
Если при запуске приложение в папке указанной через -v база данных не создаётся,
или не может открыться существующая и сыпится ошибка что база данных только для чтения
То возможным решением будет
- `cmod 777 pathToFolder` и такой же на файл
- `sudo chgrp www-data app.db` и так же на папку, я не знаю что это 
  но [Zipp](https://stackoverflow.com/questions/3319112/sqlite-error-attempt-to-write-a-readonly-database-during-insert) написал что ему помогло, и мне тоже

