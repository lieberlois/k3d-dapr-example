package analytics

import (
	"strings"
	"time"

	"AnalyticsService/models"
)

func AnalyzeWords(postData models.Post) models.Stats {
	titleSplice := strings.Split(postData.Title, " ")
	bodySplice := strings.Split(postData.Body, " ")

	// Simulate delay
	time.Sleep(time.Second * 3)

	println()
	println("Post", postData.Id)
	println("-->", len(titleSplice), "words in title")
	println("-->", len(bodySplice), "words in body")
	println()

	return models.Stats{
		PostId:     postData.Id,
		TitleCount: len(titleSplice),
		BodyCount:  len(bodySplice),
	}
}
