import React, { useState, useEffect } from 'react';
import axios from 'axios';


const TodoList = () => {
  const [todos, setTodos] = useState([]);
  const [newTodo, setNewTodo] = useState('');

  // Function to fetch all todos
  const fetchTodos = async () => {
    try {
      const response = await axios.get('http://localhost:5023/todos');
      setTodos(response.data);
    } catch (error) {
      console.error('Error fetching todos:', error);
    }
  };

  // Function to add a new todo
  const addTodo = async () => {
	console.log(newTodo)
    try {
      await axios.post('http://localhost:5023/todo', {name: newTodo });
    //   setNewTodo('');
      fetchTodos(); // Refresh the todo list after adding
    } catch (error) {
      console.error('Error adding todo:', error);
    }
  };

  // Function to update a todo : `https://localhost:5023/todo/${id}`
  const updateTodo = async (id, updatedTitle) => {
    try {
      await axios.put(`http://localhost:5023/todo/${id}`, { name: updatedTitle });
      fetchTodos(); // Refresh the todo list after updating
    } catch (error) {
      console.error('Error updating todo:', error);
    }
  };
  
  // Function to delete a todo
  const deleteTodo = async (id) => {
    try {
      await axios.delete(`http://localhost:5023/todo/${id}`);
      fetchTodos(); // Refresh the todo list after deleting
    } catch (error) {
      console.error('Error deleting todo:', error);
    }
  };

  // Fetch todos on component mount
  useEffect(() => {
    fetchTodos();
  }, []);
// console.log(newTodo)
  return (
    <div>
      <h1>Todo List</h1>
      <input
        type="text"
        value={newTodo}
        onChange={(e) => setNewTodo(e.target.value)}
        placeholder="Enter a new todo..."
      />
      <button onClick={addTodo}>Add Todo</button>

      <ul>
        {todos.map((todo) => (
          <li key={todo.id}>
            <input
              type="text"
              value={todo.name}
              onChange={(e) => updateTodo(todo.id, e.target.value)}
            />
            <button onClick={() => deleteTodo(todo.id)}>Delete</button>
          </li>
        ))}
      </ul>
    </div>
  );
};

export default TodoList;
