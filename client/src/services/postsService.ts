import axios from "axios";
import { CreatePostDto, Post } from "../models";

export const fetchPosts = async (): Promise<Array<Post>> => {
    const resp = await axios.get<Array<Post>>("http://localhost/posts");
    return resp.data
}

export const createPost = async (createPostDto: CreatePostDto): Promise<Post> => {
    const resp = await axios.post<Post>("http://localhost/posts", createPostDto);
    return resp.data
}