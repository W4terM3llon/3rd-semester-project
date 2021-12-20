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
import { OrderContext } from "./AppContext";

export default function MyOrders() {
  const { orderHistoryState } = useContext(OrderContext);

  const [orderHistory, setOrderHistory] = orderHistoryState;
  return (
    <Container>
      <Row className="mt-5 d-flex justify-content-center align-items-center">
        {orderHistory.length === 0 ? (
          <Col className="text-center">
            <h5>Order history not found</h5>
          </Col>
        ) : (
          orderHistory.map((order, index) => (
            <Col key={index} xs={12} md={6} xl={4}>
              <Card className="mb-4">
                <Card.Header>Order id: {order.id}</Card.Header>
                <Card.Body>
                  <ListGroup variant="flush">
                    <ListGroup.Item>
                      Date: {order.date.slice(0, 10)}
                    </ListGroup.Item>
                    <ListGroup.Item>
                      Time: {order.date.slice(11, 19)}
                    </ListGroup.Item>
                    <ListGroup.Item>
                      Restaurant: {order.restaurant.name}
                    </ListGroup.Item>
                    <ListGroup.Item>
                      Stage: {order.orderStage.name}
                    </ListGroup.Item>
                  </ListGroup>
                  <h5 className="mt-3 text-center">Dishes</h5>
                  <ListGroup>
                    {order.orderLines.map((ol) => (
                      <>
                        <ListGroup.Item className="d-flex justify-content-between">
                          <span>Dish: {ol.dish.name}</span>
                          <span>Price: ${ol.quantity * ol.dish.price}</span>
                          <span>Quantity: {ol.quantity}</span>
                        </ListGroup.Item>
                      </>
                    ))}
                  </ListGroup>
                </Card.Body>
              </Card>
            </Col>
          ))
        )}
      </Row>
    </Container>
  );
}
