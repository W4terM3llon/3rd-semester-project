import { useContext, useState } from "react";
import {
  Card,
  Button,
  Form,
  FloatingLabel,
  Container,
  FormGroup,
  Alert,
} from "react-bootstrap";
import { OrderContext } from "./AppContext";

export default function Dish({ dish }) {
  const { orderLinesState } = useContext(OrderContext);
  const [orderLines, setOrderLines] = orderLinesState;

  const [quantity, setQuantity] = useState(
    orderLines.filter((ol) => ol.dish.id === dish.id).length !== 0
      ? orderLines.filter((ol) => ol.dish.id === dish.id)[0].quantity
      : 1
  );
  const [showItemNotAdded, setShowItemNotAdded] = useState(false);

  console.log(orderLines);

  function isDishInOrderLines() {
    return orderLines.filter((ol) => ol.dish.id == dish.id).length != 0;
  }

  function onButtonClick() {
    if (isDishInOrderLines()) {
      let orderLineToModify = orderLines.filter(
        (ol) => ol.dish.id == dish.id
      )[0];
      if (quantity === 0) {
        setOrderLines([...orderLines.filter((ol) => ol !== orderLineToModify)]);
        setQuantity(1);
        setShowItemNotAdded(false);
      } else {
        orderLineToModify.quantity = quantity;
        setOrderLines([
          ...orderLines.filter((ol) => ol !== orderLineToModify),
          orderLineToModify,
        ]);
        setShowItemNotAdded(false);
      }
    } else {
      if (quantity === 0) {
        setQuantity(1);
      } else if (
        orderLines.length !== 0 &&
        orderLines[0].dish.restaurant.id !== dish.restaurant.id
      ) {
        setShowItemNotAdded(true);
      } else {
        setOrderLines([...orderLines, { dish: dish, quantity: quantity }]);
        setShowItemNotAdded(false);
      }
    }
  }

  return (
    <Card>
      <Card.Body>
        <Card.Title>{dish.name}</Card.Title>
        <Card.Subtitle>
          {dish.restaurant.name}, {dish.dishCategory.name}
        </Card.Subtitle>
        <Card.Text>{dish.description}</Card.Text>
        <span>{dish.price}</span>
        <Form className="d-flex justify-content-end">
          <Alert
            variant="warning"
            className="mt-2"
            show={showItemNotAdded}
            onClose={() => setShowItemNotAdded(false)}
            dismissible
          >
            <Alert.Heading>
              Can not mix dishes from different restaurants in one order
            </Alert.Heading>
          </Alert>
          <FormGroup>
            <Form.Select
              className=""
              value={quantity}
              onChange={(e) => {
                setQuantity(parseInt(e.target.value));
                console.log(e.target.value);
              }}
            >
              {[...Array(100).keys()]
                .map((i) => i + 0)
                .map((quantity) => (
                  <option>{quantity}</option>
                ))}
            </Form.Select>
          </FormGroup>
          <Button
            variant={isDishInOrderLines() ? (orderLines.filter(ol => ol.dish.id === dish.id)[0].quantity !== quantity ? "warning" : "success") : "primary"}
            onClick={() => onButtonClick()}
            className="ms-2"
            style={{ width: "150px" }}
          >
            {isDishInOrderLines() ? "Update" : "Add to cart"}
          </Button>
        </Form>
      </Card.Body>
    </Card>
  );
}
