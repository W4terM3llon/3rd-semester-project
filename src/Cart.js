import { useContext, useState } from "react";
import {
  Card,
  Button,
  Form,
  FloatingLabel,
  Container,
  FormGroup,
  Alert,
  Row,
  Col,
} from "react-bootstrap";
import { Link } from "react-router-dom";
import { JwtTokenContext, OrderContext, UserContext } from "./AppContext";
import Dish from "./Dish";
import configData from "./config.json";

export default function Cart() {
  const { jwtTokenState } = useContext(JwtTokenContext);
  const { userIdState } = useContext(UserContext);
  const { orderLinesState, fetchOrderHistory } = useContext(OrderContext);

  const [jwtToken, setJwtToken] = jwtTokenState;
  const [userId, setUserId] = userIdState;
  const [orderLines, setOrderLines] = orderLinesState;

  const [showNotLoggedin, setShowNotLoggedin] = useState(jwtToken == "");
  const [showOrderError, setShowOrderError] = useState(false);
  const [showOrderSuccess, setShowOrderSuccess] = useState(false);

  function confirmOrder() {
    fetch(`${configData.SERVER_URL}api/Orders/`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${jwtToken}`,
      },
      body: JSON.stringify({
        customer: userId,
        orderLines: orderLines.map((ol) => {
          return { dish: ol.dish.id, quantity: ol.quantity };
        }),
        restaurant: orderLines[0].dish.restaurant.id,
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
              setShowOrderError(true);
              setShowOrderSuccess(false);
            })
        );
      })
      .then((data) => {
        if (data.ok) {
          setOrderLines([]);
          fetchOrderHistory();
          setShowOrderError(false);
          setShowOrderSuccess(true);
        } else {
          setShowOrderError(true);
          setShowOrderSuccess(false);
        }
      })
      .catch((err) => {});
  }

  return (
    <Container className="mt-5 d-flex justify-content-center">
      <Col xs={12} md={9} xl={6}>
        <Alert variant="warning" className="mt-2" show={showNotLoggedin}>
          <Alert.Heading>Please login to make an order</Alert.Heading>
          <p>
            Proceed to <Link to="/login">login</Link> or{" "}
            <Link to="/signup">sign-up</Link> page
          </p>
        </Alert>
        {orderLines.length === 0 ? (
          <h5 className="text-center">Cart empty</h5>
        ) : (
          <>
            {orderLines.map((ol) => (
              <Row className="mb-2">
                <Col>
                  <Dish dish={ol.dish} />
                </Col>
              </Row>
            ))}
            <Row>
              <Col className="d-flex justify-content-end">
                <Button
                  className="primary"
                  onClick={confirmOrder}
                  disabled={jwtToken === "" || orderLines.length === 0}
                >
                  Confirm
                </Button>
              </Col>
            </Row>
          </>
        )}
        <Row>
          <Alert
            variant="warning"
            className="mt-2"
            show={showOrderError}
            onClose={() => setShowOrderError(false)}
            dismissible
          >
            <Alert.Heading>Could not place an order</Alert.Heading>
          </Alert>
        </Row>
        <Row>
          <Alert
            variant="success"
            className="mt-2"
            show={showOrderSuccess}
            onClose={() => setShowOrderSuccess(false)}
            dismissible
          >
            <Alert.Heading>Order placed</Alert.Heading>
          </Alert>
        </Row>
      </Col>
    </Container>
  );
}
