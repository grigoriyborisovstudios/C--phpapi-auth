<?php 
header('Content-Type: application/json'); 

$db = new PDO('mysql:host=localhost;dbname=dbname;charset=utf8', 'user', 'pass'); 

$data = json_decode(file_get_contents('php://input'), true);
$username = filter_var($data['username'], FILTER_SANITIZE_STRING);
$password = filter_var($data['password'], FILTER_SANITIZE_STRING);

$ip = $_SERVER['REMOTE_ADDR'];
$file = 'ratelimits.txt';
if (!file_exists($file)) {
    file_put_contents($file, '');
}
$lines = file($file);
$limit = 10;  
$timeout = 60;

foreach ($lines as $line) {
    list($old_ip, $old_time) = explode(',', $line);
    if ($old_ip == $ip && time() - $old_time < $timeout) {
        die(json_encode(['success' => false, 'message' => 'Rate limit exceeded']));
    }
}

file_put_contents($file, "$ip," . time() . "\n", FILE_APPEND);

if ($_SERVER['REQUEST_METHOD'] === 'POST') { 
    try {
        if ($_GET['url'] === "register") { 
            $stmt = $db->prepare('INSERT INTO users (username, password) VALUES (?, ?)');
            $stmt->bindParam(1, $username);
            $stmt->bindParam(2, password_hash($password, PASSWORD_DEFAULT));
            $result = $stmt->execute();
            echo json_encode(['success' => $result]);
        } else if ($_GET['url'] === "login") { 
            $stmt = $db->prepare('SELECT password FROM users WHERE username = ?');
            $stmt->bindParam(1, $username);
            $stmt->execute();
            $row = $stmt->fetch(PDO::FETCH_ASSOC);
            if ($row && password_verify($password, $row['password'])) {
                echo json_encode(['success' => true]);
            } else {
                echo json_encode(['success' => false]);
            }
        }
    } catch (PDOException $ex) {
        error_log($ex->getMessage());
        die(json_encode(['success' => false]));
    }
}
?>
