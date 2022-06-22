import React, { useEffect, useState } from "react";
import { Box, Button, Flex, Heading, Input, InputGroup, Link, List, ListItem, Text } from "@chakra-ui/react";
import { ExternalLinkIcon } from "@chakra-ui/icons";
import { Post } from "../models";

interface PostItemProps {
  post: Post
};

export const PostItem = ({post}: PostItemProps) => {
    return <Flex p={5} shadow="md" borderWidth="1px" key={post.id}>
        <Box flex={1}>
          <Heading fontSize="xl">ID: {post.id} - {post.title}</Heading>
          <Text>{post.body}</Text>
          <Link href={post.url} isExternal>
            {post.url} <ExternalLinkIcon mx='2px' />
          </Link>
          {post.stats && (
          <Box mt={4}>
            <Text mb={1} as="em">Stats:</Text>
            <Text>Words in title: {post.stats.titleCount}</Text>
            <Text>Words in body: {post.stats.bodyCount}</Text>
          </Box>
          )}
        </Box>
      </Flex>
};
  
  