import React, { useState, useEffect } from 'react';
import { Button, TextField } from '@mui/material';
import Book from './Book'
import Grid from '@mui/material/Grid';
import { toast } from 'react-toastify';
import axios from 'axios'

const Books = () => {
    const [isbn, setIsbn] = useState("");
    const [name, setName] = useState("");
    const [price, setPrice] = useState(0.0);
    const [amountOfCopies, setAmountOfCopies] = useState(0);
    const [books, setBooks] = useState(null);

    useEffect(() => {
        getAllBooks();
      }, []);
    
    const createBook = () => {
        const requestBody = {
            isbn: isbn,
            name: name,
            price: price,
            amountOfCopies: amountOfCopies
        };
        axios.post("https://localhost:5001/Book/Create", requestBody)
            .then(response => {
                toast.success("Book created successfully");
                setIsbn("");
                setName("");
                setPrice(0.0);
                setAmountOfCopies(0);
                getAllBooks();

            })
            .catch(error => {
                toast.error(error.response.data);
            })
    }

    const getAllBooks = () => {
        axios.get("https://localhost:5001/Book/GetAll")
            .then(response => {
                console.log(response.data);
                setBooks(response.data);
            })
            .catch(error => {
                console.error(error);

            })
    }

    return (
        <div>
            <div>
                <br />
                <TextField sx={{ m: 1 }} value={isbn} label="Isbn *" onChange={(e) => setIsbn(e.target.value)} />
                <TextField sx={{ m: 1 }} value={name} label="Name *" onChange={(e) => setName(e.target.value)} />
                <TextField sx={{ m: 1 }} value={price} label="Price *" type="number" onChange={(e) => setPrice(e.target.value)} />
                <TextField sx={{ m: 1 }} value={amountOfCopies} type="number" label="Amount of copies *" onChange={(e) => setAmountOfCopies(e.target.value)} />
                <br />
                <Button variant="text" onClick={createBook}>Create Book</Button>
            </div>
            <Grid container spacing={4}>
                {books && books.map((book, index) => (
                    <Grid key={index} item md={2}>
                        <Book book={book} refreshBooks={getAllBooks} />
                    </Grid>
                ))}
            </Grid>
        </div>
    );
}

export default Books;