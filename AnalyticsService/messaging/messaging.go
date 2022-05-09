package messaging

import (
	"AnalyticsService/analytics"
	"AnalyticsService/models"
	"context"
	"log"

	dapr "github.com/dapr/go-sdk/client"
	"github.com/dapr/go-sdk/service/common"
)

func EventHandler(ctx context.Context, e *common.TopicEvent) (retry bool, err error) {

	log.Printf("Got data: %v with type %T", e.Data, e.Data)

	postData, ok := e.Data.(map[string]interface{})

	if !ok {
		log.Fatal("Corrupt post data")
	}

	id := int(postData["id"].(float64))
	title, _ := postData["title"].(string)
	body, _ := postData["body"].(string)

	stats := analytics.AnalyzeWords(
		models.Post{
			Id:    id,
			Title: title,
			Body:  body,
		},
	)

	publishStats(ctx, stats)

	return false, nil
}

func publishStats(ctx context.Context, stats models.Stats) {

	client, err := dapr.NewClient()

	if err != nil {
		panic(err)
	}

	log.Printf("Publishing stats for post %d", stats.PostId)

	if err := client.PublishEventfromCustomContent(ctx, "pubsub", "stats", stats); err != nil {
		panic(err)
	}
}
