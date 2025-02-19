# DolusoftCase

# Log Parsing and Compression Service

This project contains two services: 

1. **Parser Service**: This service parses log data and pushes the parsed data to a Redis server.
2. **Compress Service**: This service subscribes to Redis, processes the data, and writes it to a JSON file. It also compresses the output using the Zstandard (Zstd) algorithm for efficient storage.

The project uses Docker to containerize both services, and Redis is used for message queuing and communication between the services.

## Description

### Parser Service
The **Parser Service** read the log data from the file, processes the logs (e.g., parses), and pushes the parsed data into a Redis queue.

### Compress Service
The **Compress Service** subscribes to the Redis queue, receives the data, writes it into a JSON file, and then compresses the file using the Zstd compression algorithm for optimized storage and transfer.

Both services run in Docker containers, which allows for easy deployment and management. The services communicate with each other via a Redis instance, which serves as a message broker between the services.

## Prerequisites

- Docker: Ensure Docker is installed and running on your machine. You can download it from [here](https://www.docker.com/get-started).
- Docker Compose: Docker Compose should be installed for managing multi-container Docker applications. You can follow the installation instructions [here](https://docs.docker.com/compose/install/).
- .NET 9.0 SDK: If you prefer to run the project without Docker, you will need to install .NET 9.0 SDK. You can get it from [here](https://dotnet.microsoft.com/download/dotnet).

## How to Run the Project

### Running with Docker

Make sure you're in the root directory where the `docker-compose.yml` file is located. Run the following command to build and start the services:

```bash
docker-compose up --build
```

This will:
- Build the images for both the Parser Service and the Compress Service
- Start the containers for both services and the Redis server
- Expose the services on the following ports:
  - Parser Service: 8080
  - Compress Service: 8081
  - Redis: 6379 (default Redis port)

### Running without Docker

If you'd prefer to run the services without Docker, you can run them directly using the .NET CLI. Follow these steps:

1. **Clone the Repository:**
   ```bash
   git clone https://github.com/DenizSAHIN570/DolusoftCase
   cd <repository-directory>
   ```

2. **Install Dependencies:**
   
   For Parser Service:
   ```bash
   cd ParserService
   dotnet restore
   ```

   For Compress Service:
   ```bash
   cd CompressService
   dotnet restore
   ```

3. **Run Redis Locally:**
   
   You can run Redis locally if you prefer not to use Docker. Use a package manager like Homebrew (for macOS) or Chocolatey (for Windows) to install Redis, or you can follow the instructions on the Redis website.

   Start Redis on the default port 6379:
   ```bash
   redis-server
   ```

4. **Run the Services:**
   
   In separate terminals, navigate to the service directories and run the services using .NET CLI:

   For Parser Service:
   ```bash
   cd ParserService
   dotnet run
   ```

   For Compress Service:
   ```bash
   cd CompressService
   dotnet run
   ```
