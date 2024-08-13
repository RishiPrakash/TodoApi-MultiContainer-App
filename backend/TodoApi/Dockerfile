# Use the official .NET Core SDK image as a base image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Set the working directory inside the container
WORKDIR /src

# Copy the project file and restore dependencies
COPY TodoApi.csproj .
RUN dotnet restore

# Copy the application files
COPY . .

# Build the application
RUN dotnet publish -c Release -o /app

# Use the official .NET Core runtime image as the final image
FROM mcr.microsoft.com/dotnet/aspnet:8.0-bookworm-slim-amd64 AS runtime

# Set the working directory inside the container
WORKDIR /app

# Copy the published application
COPY --from=build /app .

# Expose the port - the port on which the container will be accessible
EXPOSE 3000

# Expose the URL on which the application http server will listen - should be same as of
# container exposed port
ENV ASPNETCORE_URLS=http://+:3000

# Set the entry point
ENTRYPOINT ["dotnet", "TodoApi.dll"]