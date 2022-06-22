import { Post } from "./post";

export type CreatePostDto = Pick<Post, 'title' | 'body'>