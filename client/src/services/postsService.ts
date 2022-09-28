import axios from "axios";
import { CreatePostDto, Post } from "../models";

export const fetchPosts = async (): Promise<Array<Post>> => {
    const resp = await axios.get<Array<Post>>("http://localhost:8080/posts");
    return resp.data
}

export const createPost = async (createPostDto: CreatePostDto): Promise<Post> => {
    const resp = await axios.post<Post>("http://localhost:8080/posts", createPostDto);
    return resp.data
}