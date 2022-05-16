import { Button} from '@mui/material';
import React from 'react';
import { Link } from "react-router-dom";

const Navigation = () => {
    return (
        <div>
            <Link to="/books"><Button variant="text">Books</Button></Link>
        </div>
    );
}

export default Navigation;