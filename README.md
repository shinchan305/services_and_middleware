## Use Hosted Images
- Download the `docker-compose.yml` file within Accounts.API.
- Run the command `docker-compose up`
- Since RabbitMQ takes a bit of time to run, `pdfservice`, `notificationservice1` and `notificationservice2` will fail and exit. Please run these 3 containers again once rabbitmq is up.

## Run the project locally
- Clone this github repo.
- Open the directory in your local command prompt window
- Ensure that your docker is running
- Type the command `docker-compose up --build`

## Calling Endpoints
- Go to your browser and navigate to `http://localhost:5002/swagger/index.html`
- Use the `Try It Out` and check the docker logs of respective services to see all the scenarios.
