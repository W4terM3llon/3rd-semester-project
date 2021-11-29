import { Container } from "react-bootstrap";
import { BrowserRouter as Router, Routes, Route, Link } from "react-router-dom";
import Navigation from "./Navigation";
import Login from "./Login";
import Signup from "./Signup";
import Restaurants from "./Restaurants";
import RestaurantDetails from "./RestaurantDetails"
import BookingConfirmation from "./BookingConfirmation";

function App() {
  return (
    <>
      <Router>
        <Navigation />
        <Container>
          <Routes>
            <Route exact path="/home" element={<Restaurants />} />
            <Route exact path="/restaurant-details" element={<RestaurantDetails />} />
            <Route exact path="/booking-confirmation" element={<BookingConfirmation />} />
            <Route exact path="/login" element={<Login />} />
            <Route exact path="/signup" element={<Signup />} />
          </Routes>
        </Container>
      </Router>
    </>
  );
}

export default App;
