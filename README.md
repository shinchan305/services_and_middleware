# Running The Poject

We can set up the project in 2 ways:

## Using Hosted Images
- Please ensure Docker is running on your local machine.
- Download the `docker-compose.yml` file within `Accounts.API` folder.
- Run the command `docker-compose up`.
- Wait for all the containers to be up before calling the APIs.

## Setting up locally
- Ensure that your docker is running
- Clone this github repo.
- Open the directory in your local command prompt window.
- Type the command `docker-compose up --build`

# Calling Endpoints
- Go to your browser and navigate to `http://localhost:5002/swagger/index.html`
- Use the `Try It Out` and check the docker logs of respective services to see all the scenarios.
