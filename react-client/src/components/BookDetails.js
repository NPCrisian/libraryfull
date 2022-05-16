import React, { useState, useEffect } from 'react';
import { Button, TextField } from '@mui/material';
import Table from '@mui/material/Table';
import TableBody from '@mui/material/TableBody';
import TableCell from '@mui/material/TableCell';
import TableContainer from '@mui/material/TableContainer';
import TableHead from '@mui/material/TableHead';
import TableRow from '@mui/material/TableRow';
import Box from '@mui/material/Box';
import Modal from '@mui/material/Modal';
import { useParams } from "react-router-dom";
import { toast } from 'react-toastify';
import axios from 'axios'
import moment from 'moment';
import Typography from '@mui/material/Typography';
import Paper from '@mui/material/Paper';

const BookDetails = () => {
    const { bookId } = useParams();
    const [returnDate, setReturnDate] = useState("2022-05-15");
    const [book, setBook] = useState(null);
    const [finishReservationId, setFinishReservationId] = useState(null);
    const [modalOpen, setModalOpen] = React.useState(false);

    const style = {
        position: 'absolute',
        top: '50%',
        left: '50%',
        transform: 'translate(-50%, -50%)',
        width: 400,
        bgcolor: 'background.paper',
        boxShadow: 24,
        pt: 2,
        px: 4,
        pb: 3,
    };

    useEffect(() => {
        getBookDetails();
    }, []);

    const openReturnModal = (reservationId) => {
        console.log(reservationId);
        setFinishReservationId(reservationId);
        setModalOpen(true);
    };

    const closeReturnModal = () => {
        setModalOpen(false);
    };

    const finishReservation = () => {
        const requestBody = {
            reservationId: finishReservationId,
            returnDateTime: returnDate
        };
        axios.post("https://localhost:5001/Reservation/ReturnBook", requestBody)
            .then(response => {
                toast.success("Book returned successfully");
                setReturnDate("2022-05-15");
                closeReturnModal();
                getBookDetails();
            })
            .catch(error => {
                console.error(error);
                toast.error(error.response.data);
            })
    };

    const getBookDetails = () => {
        axios.get("https://localhost:5001/Book/Get/" + bookId)
            .then(response => {
                setBook(response.data)
            })
            .catch(error => {
                console.error(error);
            })
    }

    return (
        <div>
            {book &&
                <div>
                    <Box sx={{ width: '100%', maxWidth: 500 }}>
                        <Typography variant="h4" gutterBottom component="div">
                            {book.name}
                        </Typography>
                    </Box>
                    <Modal
                        hideBackdrop
                        open={modalOpen}
                        onClose={closeReturnModal}
                        aria-labelledby="child-modal-title"
                        aria-describedby="child-modal-description"
                    >
                        <Box sx={{ ...style, width: 200 }}>
                            <h2 id="child-modal-title">Choose Date</h2>
                            <TextField sx={{ m: 1 }} value={returnDate} label="Return Date *" type="date" onChange={(e) => setReturnDate(e.target.value)} />
                            <Button onClick={closeReturnModal}>Close</Button>
                            <Button onClick={finishReservation}>Return</Button>
                        </Box>
                    </Modal>
                    <Typography variant="h5">Active Reservations</Typography>
                    <TableContainer component={Paper}>
                        <Table sx={{ minWidth: 650 }} aria-label="simple table">
                            <TableHead>
                                <TableRow>
                                    <TableCell>Id</TableCell>
                                    <TableCell align="right">Created At</TableCell>
                                    <TableCell align="right">Initial Book Price</TableCell>
                                    <TableCell align="right">Return</TableCell>
                                </TableRow>
                            </TableHead>
                            <TableBody>
                                {book.activeReservations.map((reservation, index) => (
                                    <TableRow
                                        key={index}
                                        sx={{ '&:last-child td, &:last-child th': { border: 0 } }}
                                    >
                                        <TableCell align="left">{reservation.id}</TableCell>
                                        <TableCell align="right">{moment(reservation.dateTime).format("DD/MM/YYYY")}</TableCell>
                                        <TableCell align="right">{reservation.initialBookPrice}</TableCell>
                                        <TableCell align="right"><Button onClick={(e) => openReturnModal(reservation.id)}>Return</Button></TableCell>
                                    </TableRow>
                                ))}
                            </TableBody>
                        </Table>
                    </TableContainer>
                    <Typography variant="h5">Finished Reservations</Typography>
                    <TableContainer component={Paper}>
                        <Table sx={{ minWidth: 650 }} aria-label="simple table">
                            <TableHead>
                                <TableRow>
                                    <TableCell>Id</TableCell>
                                    <TableCell align="right">Created At</TableCell>
                                    <TableCell align="right">Initial Book Price</TableCell>
                                    <TableCell align="right">Returned At</TableCell>
                                    <TableCell align="right">Fine</TableCell>
                                </TableRow>
                            </TableHead>
                            <TableBody>
                                {book.finishedReservations.map((reservation, index) => (
                                    <TableRow
                                        key={index}
                                        sx={{ '&:last-child td, &:last-child th': { border: 0 } }}
                                    >
                                        <TableCell align="left">{reservation.id}</TableCell>
                                        <TableCell align="right">{moment(reservation.dateTime).format("DD/MM/YYYY")}</TableCell>
                                        <TableCell align="right">{reservation.initialBookPrice}</TableCell>
                                        <TableCell align="right">{moment(reservation.returnDateTime).format("DD/MM/YYYY")}</TableCell>
                                        <TableCell align="right">{reservation.fine}</TableCell>
                                    </TableRow>
                                ))}
                            </TableBody>
                        </Table>
                    </TableContainer>
                </div>
            }
        </div>
    );
}

export default BookDetails;