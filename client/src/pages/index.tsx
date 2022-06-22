import { ExternalLinkIcon } from "@chakra-ui/icons";
import { Box, Button, Flex, Heading, Input, InputGroup, Link, List, ListItem, Text } from "@chakra-ui/react";
import React, { useEffect, useState } from "react";
import { PostItem } from "../components";
import { CreatePostDto, Post } from "../models";
import { createPost, fetchPosts } from "../services/postsService";

const Index = () => {

  const [posts, setPosts] = useState<Array<Post>>([])

  const [title, setTitle] = useState<string>('')
  const [body, setBody] = useState<string>('')

  const loadPosts = () => {
    fetchPosts()
      .then((posts: Array<Post>): void => {
        // ORDER BY id DESC (newest first!)
        setPosts(posts.sort((p1: Post, p2: Post) => p2.id - p1.id))
      })
      .catch(console.log)
  }

  const onSubmit = () => {
    const dto: CreatePostDto = {
      title,
      body
    };

    createPost(dto).then((p: Post) => setPosts(
      [p, ...posts].sort((p1: Post, p2: Post) => p2.id - p1.id)
    ));

    setTitle('');
    setBody('');
  }

  useEffect((): void => {
    loadPosts();
  }, [])

  return (
    <Box padding={5}>
      <Flex justifyContent="space-evenly" maxHeight="80%">
        <Box>
          <List spacing={5}>
            {(posts && posts.length > 0) ? posts.map(p => 
              <ListItem key={p.id}>
                <PostItem post={p} />
              </ListItem>
            ) : <Heading>No posts found.</Heading>}
          </List>
        </Box>
        <Box>
          <InputGroup size='lg'>
            <Flex flexDirection="column">
              <Input placeholder='Title' value={title} onChange={(e) => setTitle(e.target.value)} />
              <Input placeholder='Body' value={body} onChange={(e) => setBody(e.target.value)} />
              <Button colorScheme='teal' onClick={onSubmit}>
                Submit
              </Button>
              <Button colorScheme='gray' onClick={loadPosts}>
                Refresh Posts
              </Button>
            </Flex>
          </InputGroup>
        </Box>
      </Flex>
    </Box>
  )
}

export default Index
