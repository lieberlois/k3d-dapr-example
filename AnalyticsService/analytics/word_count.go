package analytics

import (
	"strings"

	"AnalyticsService/models"
)

func AnalyzeWords(postData models.Post) {
	titleSplice := strings.Split(postData.Title, " ")
	bodySplice := strings.Split(postData.Body, " ")

	println()
	println("Post", postData.Id)
	println("-->", len(titleSplice), "words in title")
	println("-->", len(bodySplice), "words in body")
	println()
}
