<?php 
header('Content-Type: application/json'); 

$db = new PDO('mysql:host=localhost;dbname=dbname;charset=utf8', 'user', 'pass'); 

$data = json_decode(file_get_contents('php://input'), true);
$username = $data['username'];
$password = $data['password'];

if ($_SERVER['REQUEST_METHOD'] === 'POST') { 
    if (strpos($_SERVER['REQUEST_URI'], '/register') !== false) { 
        $stmt = $db->prepare('INSERT INTO users (username, password) VALUES (?, ?)'); 
        $result = $stmt->execute([$username, password_hash($password, PASSWORD_DEFAULT)]); 
        echo json_encode(['success' => $result]); 
    } 
    elseif (strpos($_SERVER['REQUEST_URI'], '/login') !== false) { 
        $stmt = $db->prepare('SELECT password FROM users WHERE username = ?'); 
        $stmt->execute([$username]); 
        $result = $stmt->fetch(PDO::FETCH_ASSOC); 

        if ($result && password_verify($password, $result['password'])) { 
            echo json_encode(['success' => true]); 
        } 
        else { 
            echo json_encode(['success' => false]); 
        } 
    } 
}