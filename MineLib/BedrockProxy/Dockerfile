FROM alpine/git
WORKDIR /app
RUN git clone https://github.com/GeyserMC/Geyser.git .

FROM maven:3-jdk-8-alpine
WORKDIR /app
COPY --from=0 /app /app
RUN mvn install

FROM openjdk:8u212-jre
WORKDIR /app
COPY --from=1 /app/target/Geyser.jar /app
CMD java -jar Geyser.jar