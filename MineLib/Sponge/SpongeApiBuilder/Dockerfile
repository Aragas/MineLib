FROM alpine/git
WORKDIR /app
RUN git clone https://github.com/SpongePowered/SpongeAPI.git .
RUN cp scripts/pre-commit .git/hooks

#FROM openjdk:8-jdk-alpine3.9
#WORKDIR /app
#COPY --from=0 /app /app
#RUN ./gradlew
#ENV GRADLE_HOME=/opt/gradle/gradle-4.10.3
#ENV PATH=$PATH:$GRADLE_HOME/bin
#RUN gradle

FROM frekele/gradle:3.3-jdk8
WORKDIR /app
COPY --from=0 /app /app
RUN ./gradlew
CMD gradle