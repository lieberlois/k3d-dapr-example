package main

import (
	"AnalyticsService/messaging"
	"log"
	"net/http"

	"github.com/dapr/go-sdk/service/common"
	daprd "github.com/dapr/go-sdk/service/http"
)

var postsSubscription = &common.Subscription{
	PubsubName: "pubsub",
	Topic:      "posts",
	Route:      "/posts",
}

func main() {
	log.Println("Connecting to DAPR sidecar...")
	s := daprd.NewService(":6000")

	if err := s.AddTopicEventHandler(postsSubscription, messaging.EventHandler); err != nil {
		log.Fatalf("error adding topic subscription: %v", err)
	}

	if err := s.Start(); err != nil && err != http.ErrServerClosed {
		log.Fatalf("error listening: %v", err)
	}
}
