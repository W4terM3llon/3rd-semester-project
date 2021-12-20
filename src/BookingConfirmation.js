import { useContext, useState } from "react";
import {
  Card,
  ListGroup,
  Button,
  Col,
  Container,
  Alert,
  Row,
} from "react-bootstrap";
import { Link } from "react-router-dom";
import {
  BookingContext,
  ChosenRestaurantContext,
  JwtTokenContext,
  UserContext,
} from "./AppContext";
import configData from "./config.json";
import { getTimeString } from "./services";

export default function BookingConfirmation() {
  const { jwtTokenState } = useContext(JwtTokenContext);
  const { userIdState } = useContext(UserContext);
  const {
    bookingTableState,
    bookingTimePeriodState,
    bookingDateState,
    fetchBookingHistory,
  } = useContext(BookingContext);
  const { fetchTablesFreePeriods, chosenRestaurantTablesState } = useContext(
    ChosenRestaurantContext
  );

  const [jwtToken, setJwtToken] = jwtTokenState;
  const [userId, setUserId] = userIdState;

  const [bookingDate, setBookingDate] = bookingDateState;
  const [bookingTable, setBookingTable] = bookingTableState;
  const [bookingTimePeriod, setBookingTimePeriod] = bookingTimePeriodState;

  const [chosenRestaurantTables, setChosenRestaurantTables] =
    chosenRestaurantTablesState;

  const [showNotLoggedin, setShowNotLoggedin] = useState(jwtToken == "");
  const [showBookingError, setShowBookingError] = useState(false);
  const [showBookingSuccess, setShowBookingSuccess] = useState(false);

  function confirmBooking() {
    if (!bookingDate || !bookingTable || !bookingTimePeriod || !userId) {
      setShowBookingError(true);
      setShowBookingSuccess(false);
      return;
    }

    fetch(`${configData.SERVER_URL}api/Booking/`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${jwtToken}`,
      },
      body: JSON.stringify({
        date: bookingDate,
        table: bookingTable.id,
        diningPeriod: bookingTimePeriod.id,
        user: userId,
        restaurant: bookingTable.restaurant.id,
      }),
    })
      .then((response) => {
        return new Promise((resolve) =>
          response
            .json()
            .then((data) =>
              resolve({ ok: response.ok, status: response.status, body: data })
            )
            .catch((err) => {
              setShowBookingError(true);
            })
        );
      })
      .then((data) => {
        if (data.ok) {
          setBookingDate(new Date().toISOString().split("T")[0]);
          setBookingTable("");
          setBookingTimePeriod("");
          fetchTablesFreePeriods(chosenRestaurantTables);
          setShowBookingError(false);
          setShowBookingSuccess(true);

          fetchBookingHistory();
        } else {
          setShowBookingError(true);
          setShowBookingSuccess(false);
        }
      })
      .catch((err) => {});
  }

  return (
    <Container className="d-flex justify-content-center align-items-center mt-5">
      <Col xs={12} md={8} xl={4}>
        <Row>
          <Alert variant="warning" className="mt-2" show={showNotLoggedin}>
            <Alert.Heading>Please login to make a booking</Alert.Heading>
            <p>
              Proceed to <Link to="/login">login</Link> or{" "}
              <Link to="/signup">sign-up</Link> page
            </p>
          </Alert>
        </Row>
        <Row>
          <Card className="p-3">
            <Card.Title className="text-center">
              Confirm your booking
            </Card.Title>
            <ListGroup>
              <ListGroup.Item>Date: {bookingDate}</ListGroup.Item>
              <ListGroup.Item>
                Time:{" "}
                {bookingTimePeriod.timeStartMinutes ? (
                  getTimeString(bookingTimePeriod.timeStartMinutes)
                ) : (
                  <span className="text-danger">*Choose time period</span>
                )}
              </ListGroup.Item>
              <ListGroup.Item>
                Restaurant:{" "}
                {bookingTable.restaurant ? (
                  bookingTable.restaurant.name
                ) : (
                  <span className="text-danger">
                    *Choose restaurant's table
                  </span>
                )}
              </ListGroup.Item>
              <ListGroup.Item>
                Table seats:{" "}
                {bookingTable.restaurant ? (
                  bookingTable.seatNumber
                ) : (
                  <span className="text-danger">*Choose table</span>
                )}
              </ListGroup.Item>
            </ListGroup>
            <Button
              onClick={confirmBooking}
              className="mt-2"
              disabled={jwtToken === ""}
            >
              Confirm
            </Button>
          </Card>
        </Row>
        <Row>
          <Alert
            variant="warning"
            className="mt-2"
            show={showBookingError}
            onClose={() => setShowBookingError(false)}
            dismissible
          >
            <Alert.Heading>Could not place a booking</Alert.Heading>
          </Alert>
          <Alert
            variant="success"
            className="mt-2"
            show={showBookingSuccess}
            onClose={() => setShowBookingSuccess(false)}
            dismissible
          >
            <Alert.Heading>Successfull booking</Alert.Heading>
          </Alert>
        </Row>
      </Col>
    </Container>
  );
}
