import { Container } from "react-bootstrap";
import { BrowserRouter as Router, Routes, Route, Link } from "react-router-dom";
import Navigation from "./Navigation";
import Login from "./Login";
import Signup from "./Signup";
import Restaurants from "./Restaurants";
import RestaurantDetails from "./RestaurantDetails"
import BookingConfirmation from "./BookingConfirmation";
import MyAccount from "./MyAccount";
import MyBookings from "./MyBookings";
import MyOrders from "./MyOrders";
import Cart from "./Cart";

function App() {
  return (
    <>
      <Router>
        <Navigation />
        <Container>
          <Routes>
            <Route exact path="/home" element={<Restaurants />} />
            <Route exact path="/restaurant/:id" element={<RestaurantDetails />} />
            <Route exact path="/booking-confirmation" element={<BookingConfirmation />} />
            <Route exact path="/cart" element={<Cart />} />
            <Route exact path="/my-order-history" element={<MyOrders />} />
            <Route exact path="/my-booking-history" element={<MyBookings />} />
            <Route exact path="/my-Account" element={<MyAccount />} />
            <Route exact path="/login" element={<Login />} />
            <Route exact path="/sign-up" element={<Signup />} />
          </Routes>
        </Container>
      </Router>
    </>
  );
}

export default App;
