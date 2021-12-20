import { useContext, useEffect, useState } from "react";
import {
  Card,
  Col,
  Container,
  Form,
  ListGroup,
  Row,
  Tab,
} from "react-bootstrap";
import { BookingContext } from "./AppContext";
import { getTimeString } from "./services";

export default function MyBookings() {
  const { bookingHistoryState } = useContext(BookingContext);

  const [bookingHistory, setBookingHistory] = bookingHistoryState;

  return (
    <Container>
      <Row className="mt-5 d-flex justify-content-center align-items-center">
        {
          (bookingHistory.length === 0 ? (
              <Col className="text-center">
                <h5>Booking not found</h5>
              </Col>
          ) : (
            bookingHistory.map((booking, index) => (
              <Col key={index} xs={12} md={6} xl={4}>
                <Card className="mb-4">
                  <Card.Header>Booking id: {booking.id}</Card.Header>
                  <Card.Body>
                    <ListGroup variant="flush">
                      <ListGroup.Item>
                        Date: {booking.date.slice(0, 10)}
                      </ListGroup.Item>
                      <ListGroup.Item>
                        Time: {getTimeString(booking.diningPeriod.timeStartMinutes)}
                      </ListGroup.Item>
                      <ListGroup.Item>
                        Table id: {booking.table.id}
                      </ListGroup.Item>
                      <ListGroup.Item>
                        Table seats: {booking.table.seatNumber}
                      </ListGroup.Item>
                      <ListGroup.Item>
                        Restaurant: {booking.restaurant.name}
                      </ListGroup.Item>
                    </ListGroup>
                  </Card.Body>
                </Card>
              </Col>
            ))
          ))
        }
      </Row>
    </Container>
  );
}
