# Curso Alura Microsserviços e .NET 6: implementando a comunicação 

### Capítulo 3

Para criar um network customizado onde os containers da aplicação podem se comunicar entre si
`docker network create --driver bridge restaurante-bridge`

Para rodar os containers com o nome e network customizados
`docker run --name item-service -d -p 8080:80 --network restaurante-bridge itemservice:1.1`
`docker run --name=mysql -e MYSQL_ROOT_PASSWORD=root -d --network restaurante-bridge mysql:5.6`
`docker run --name restaurante-service -p 8081:80 --network restaurante-bridge restauranteservice:1.4`

E por fim, no appsettings do RestauranteService mudar as URL de localhost/ip dos containers para o nome de cada container dado no atributo `--name`. Dessa forma, não precisamos nos preocupar com a mudança de ip dinâmica dos containers, fazendo a comunicação entre eles de forma similar a um DNS.