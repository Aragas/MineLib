FROM alpine/git AS git
WORKDIR /app
RUN git clone --depth=1 --branch=master https://github.com/GeyserMC/Geyser.git .

FROM maven:3-jdk-8-alpine AS build
WORKDIR /app
COPY --from=git /app /app
RUN mvn install

FROM openjdk:8u212-jre AS final
WORKDIR /app
COPY --from=build /app/target/Geyser.jar /app
CMD java -jar Geyser.jar