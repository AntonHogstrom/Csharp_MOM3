using System;
using System.Collections.Generic;
using System.Text.Json;
using System.IO;

namespace GuestBook
{
    //Class for a post with getters and setters
    public class Post
    {
        private string content;
        public string Content
        {
            set { this.content = value; }
            get { return this.content; }
        }

        private string author;
        public string Author
        {
            set { this.author = value; }
            get { return this.author; }
        }
    }

    //Class for list, post, get, delete and serialize / deserialize
    public class GuestBook
    {
        //Filename
        private string fileName = @"guestbook.json";
        //Instance of list created
        private List<Post> PostList = new List<Post>();

        //Constructor
        public GuestBook()
        {
            //Will create file if doesnt exist, will be put in bin/Debug/net5.0
            if(File.Exists(fileName) == true)
            {
                //Assign jsonString from file to List
                string jsonString = File.ReadAllText(fileName);
                PostList = JsonSerializer.Deserialize<List<Post>>(jsonString);
            }
        }
        //Add post function
        public Post AddPost(string author, string content)
        {
            //Uses post class
            Post newPost = new Post
            {
                Author = author,
                Content = content,
            };

            //Adds instance of post class with values to List to then serialize the List
            PostList.Add(newPost);
            Serialize(); 
            return newPost;
        }

        //Delete function
        public int DeletePost(int index)
        {
            //Removes indexed object from List to then serialize List
            PostList.RemoveAt(index);
            Serialize();
            
            return index;
        }

        //Returns List
        public List<Post> GetPostList()
        {
            return PostList;
        }

        //Posts List data in format
        public int PrintList()
        {
            int i = 1;
            foreach(Post post in this.GetPostList())
            {
                Console.WriteLine($"{i}");
                Console.WriteLine("Author: " + post.Author);
                Console.WriteLine("Content: \n" + post.Content);
                Console.WriteLine("=================================");
                i++;
            }
            //If no posts
            if (i == 1)
            {
                Console.WriteLine("No Posts, be the first to make a Post!");
            }
            return i;
        }

        //Serialize List and write list to file
        private void Serialize()
        {
            var jsonString = JsonSerializer.Serialize(PostList);
            File.WriteAllText(fileName, jsonString);
        }
    }




    class Program
    {
        static void Main(string[] args)
        {
            //The instance of GuestBook
            GuestBook GuestBook = new GuestBook();

            //Infinite loop
            while (true)
            {
                Console.CursorVisible = false;

                Console.WriteLine("");
                Console.WriteLine("====================");
                Console.WriteLine("= Antons Guestbook =");
                Console.WriteLine("====================");
                Console.WriteLine("");

                //Initial menu options
                Console.WriteLine("1: Read Posts");
                Console.WriteLine("2: Write New Post");
                Console.WriteLine("3: Delete Post");
                Console.WriteLine("");
                Console.WriteLine("4: Exit program");

                //Takes in key and checks if 1, 2, 3 or 4,
                //switch case for options depending on option
                int input = (int)Console.ReadKey(true).Key;
                switch (input)
                {
                    case '1':
                        Console.CursorVisible = true;
                        Console.Clear();
                        Console.WriteLine("");
                        Console.WriteLine("==========");
                        Console.WriteLine("= POSTS: =");
                        Console.WriteLine("==========");
                        Console.WriteLine("");

                        //skriver ut lista
                        GuestBook.PrintList();
                        Console.WriteLine("Enter: Continue");
                        Console.ReadLine();
                        break;

                    case '2':
                        Console.CursorVisible = true;
                        Console.Clear();

                        string author = "";
                        string content = "";

                        //Input check Author
                        while(string.IsNullOrEmpty(author))
                        { 
                            Console.WriteLine("Declare Author ");
                            author = Console.ReadLine();
                        }
                        //Input check Content
                        while (string.IsNullOrEmpty(content))
                        {
                            Console.WriteLine("\nWrite Your Post: ");
                            content = Console.ReadLine();
                        }

                        string answer;
                        //Show post before public
                        //Choice to post or abort
                        do
                        {
                            Console.Clear();
                            Console.WriteLine("");
                            Console.WriteLine("");
                            Console.WriteLine("");
                            Console.WriteLine("Your Post as following: ");
                            Console.WriteLine("=======================");
                            Console.WriteLine($"Author: {author}");
                            Console.WriteLine("");
                            Console.WriteLine($"Content:\n\n {content}");
                            Console.WriteLine("=======================");
                            Console.WriteLine("");
                            Console.WriteLine("1: Post");
                            Console.WriteLine("2: Abort");
                            answer = Console.ReadLine();

                            if (answer == "1")
                            {
                                GuestBook.AddPost(author, content);

                                Console.Clear();
                                Console.WriteLine("Post Created");
                            }
                            else
                            {
                                answer = "2";
                                Console.Clear();
                                Console.WriteLine("Post Aborted");
                            }
                            //Controll if input 1 or 2
                        } while (answer != "1" && answer != "2");

                        break;

                        //delete
                    case '3':
                        Console.Clear();
                        //Prints list so user can see posts before delete
                        if (GuestBook.PrintList() == 1)
                        {
                            Console.WriteLine("Enter: Continue");
                            Console.ReadLine();
                            break;
                        } else
                        {
                            string indexStr = "";
                            bool isNumber;
                            int indexInt;
                            bool pass = false;

                            //Loop while pass is false
                            do
                            {
                                indexStr = "";
                                isNumber = false;
                                indexInt = 0;

                                Console.WriteLine("Declare Post to Delete");
                                indexStr = Console.ReadLine();

                                //parses string to int
                                isNumber = int.TryParse(indexStr, out indexInt);

                                //Check if input is a number, not higher than post count
                                //and not lower than 0, make pass variable true to exit loop
                                if (isNumber == true)
                                {
                                    //Checks if indexInt is equal or smaller to elements/objects in List
                                    if (indexInt <= GuestBook.GetPostList().Count)
                                    {
                                        //Checks that indexInt is not negative
                                        if (indexInt >= 0)
                                        {
                                            //Gives pass out of loop
                                            pass = true;
                                        } else {
                                            Console.WriteLine("Number cant be lower than 1");
                                            Console.WriteLine("Enter: Continue");
                                            Console.ReadLine();
                                        }
                                    } else {
                                        Console.WriteLine($"Number too high. Last post is '{GuestBook.GetPostList().Count}'");
                                        Console.WriteLine("Enter: Continue");
                                        Console.ReadLine();
                                    }
                                } else {
                                    Console.WriteLine("Not a Number");
                                    Console.WriteLine("Enter: Continue");
                                    Console.ReadLine();
                                }

                            } while (pass == false);

                            //gets here only when variable is validated to delete post
                            //-1 to match UI with backend index
                            GuestBook.DeletePost(indexInt -1);

                            Console.Clear();
                            Console.WriteLine($"Post {indexStr} deleted");
                            Console.WriteLine($"Enter: Continue");
                            Console.ReadLine();

                            break;
                        }
                        //exit Application
                    case '4':
                        Console.Clear();
                        Console.WriteLine("See you back Soon!");
                        Console.WriteLine("https://github.com/NoEz9/");
                        System.Environment.Exit(0);
                        break;
                }
            }
        }
    }
}
