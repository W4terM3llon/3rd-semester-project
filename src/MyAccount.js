import { useContext, useEffect, useState } from "react";
import {
  Card,
  Col,
  Container,
  Form,
  ListGroup,
  Row,
  Tab,
  Button,
} from "react-bootstrap";
import { UserContext } from "./AppContext";

export default function MyAccount() {
  const {
    userIdState,
    userFirstNameState,
    userLastNameState,
    userEmailState,
    userPhoneNumberState,
    userAddressStreetState,
    userAddressAppartmentState,
  } = useContext(UserContext);

  const [userId, setUserId] = userIdState;
  const [userFirstName, setUserFirstName] = userFirstNameState;
  const [userLastName, setUserLastName] = userLastNameState;
  const [userEmail, setUserEmail] = userEmailState;
  const [userPhoneNumber, setUserPhoneNumber] = userPhoneNumberState;
  const [userAddressStreet, setUserAddressStreet] = userAddressStreetState;
  const [userAddressAppartment, setUserAddressAppartment] =
    userAddressAppartmentState;

  return (
    <Container className="mt-5 d-flex justify-content-center align-items-center">
      <Col xs={12} md={9} lg={6}>
        <Row>
          <Col>
            <Card>
              <Card.Header>Account details</Card.Header>
              <Card.Body>
                <ListGroup variant="flush">
                  <h5 className="text-center">Personal information</h5>
                  <ListGroup.Item>ID: {userId}</ListGroup.Item>
                  <ListGroup.Item>First name: {userFirstName}</ListGroup.Item>
                  <ListGroup.Item>Last name: {userLastName}</ListGroup.Item>
                  <ListGroup.Item>Email: {userEmail}</ListGroup.Item>
                  <ListGroup.Item>
                    Phone number: {userPhoneNumber}
                  </ListGroup.Item>
                  <h5 className="mt-3 text-center">Address</h5>
                  <ListGroup.Item>Street: {userAddressStreet}</ListGroup.Item>
                  <ListGroup.Item>
                    Appartment: {userAddressAppartment}
                  </ListGroup.Item>
                </ListGroup>
              </Card.Body>
            </Card>
          </Col>
        </Row>
        <Row>
          <Col className="mt-3 d-flex justify-content-end">
            <Button variant="primary">update</Button>
            <Button variant="light" className="ms-3">delete</Button>
          </Col>
        </Row>
      </Col>
    </Container>
  );
}
