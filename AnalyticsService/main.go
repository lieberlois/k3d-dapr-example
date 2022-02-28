package main

import (
	"AnalyticsService/analytics"
	"AnalyticsService/models"
	"context"
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
	log.Println("Connecting to pubsub message bus...")

	s := daprd.NewService(":6000")

	if err := s.AddTopicEventHandler(postsSubscription, eventHandler); err != nil {
		log.Fatalf("error adding topic subscription: %v", err)
	}

	if err := s.Start(); err != nil && err != http.ErrServerClosed {
		log.Fatalf("error listenning: %v", err)
	}

	log.Printf("Now listening to subscription: %v", postsSubscription)
}

func eventHandler(ctx context.Context, e *common.TopicEvent) (retry bool, err error) {

	log.Printf("Got data: %v with type %T", e.Data, e.Data)

	postData, ok := e.Data.(map[string]interface{})

	if !ok {
		log.Fatal("Corrupt post data")
	}

	id, _ := postData["id"].(string)
	title, _ := postData["title"].(string)
	body, _ := postData["body"].(string)

	analytics.AnalyzeWords(
		models.Post{
			Id:    id,
			Title: title,
			Body:  body,
		},
	)

	return false, nil
}
