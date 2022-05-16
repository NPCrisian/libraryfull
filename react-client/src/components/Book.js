import * as React from 'react';
import Card from '@mui/material/Card';
import CardContent from '@mui/material/CardContent';
import Typography from '@mui/material/Typography';
import { Button,  CardActions } from '@mui/material';
import { Link } from "react-router-dom";
import { toast } from 'react-toastify';
import axios from 'axios'


const Book = ({ book, refreshBooks }) => {
    const CreateReservation = () => {
        axios.post("https://localhost:5001/Reservation/Create/"+book.id)
            .then(response => {
                refreshBooks();
                toast.success("Reservation created successfully");
            })
            .catch(error => {
                console.error(error);
                toast.error(error.response.data);
            })
    }

    return (
        <Card sx={{ maxWidth: 345 }}>
            <CardContent>
                <Typography gutterBottom variant="h5" component="div">
                    {book.name}
                </Typography>
                <Typography variant="body2" color="text.secondary">
                    {book.isbn}
                </Typography>
                <Typography variant="body2" color="text.secondary">
                    price: {book.price}
                </Typography>
                <Typography variant="body2" color="text.secondary">
                    available: {book.amountOfCopiesAvailable}/{book.amountOfCopies}
                </Typography>
            </CardContent>
            <CardActions>
                <Button variant="text" onClick={CreateReservation}>Reserve</Button>
            </CardActions>
            <CardActions>
                <Link to={'/bookDetails/' + book.id}><Button variant="text">Reservations</Button></Link>
            </CardActions>
        </Card>
    );
}

export default Book;