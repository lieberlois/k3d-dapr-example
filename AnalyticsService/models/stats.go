package models

type Stats struct {
	PostId     int `json:"postId,omitempty"`
	TitleCount int `json:"titleCount,omitempty"`
	BodyCount  int `json:"bodyCount,omitempty"`
}
