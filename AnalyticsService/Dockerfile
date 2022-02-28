FROM golang:alpine AS build
WORKDIR /src
ENV CGO_ENABLED=0
COPY . .
RUN go build -o /out/analyticsService .

FROM scratch
COPY --from=build /out/analyticsService /
ENTRYPOINT ["./analyticsService"]