<?php 
header('Content-Type: application/json'); 

$db_host = getenv('DB_HOST');
$db_name = getenv('DB_NAME');
$db_user = getenv('DB_USER');
$db_pass = getenv('DB_PASS');

$db = new PDO("mysql:host=$db_host;dbname=$db_name;charset=utf8", $db_user, $db_pass); 

$data = json_decode(file_get_contents('php://input'), true);
$username = filter_var($data['username'], FILTER_SANITIZE_STRING);
$password = filter_var($data['password'], FILTER_SANITIZE_STRING);

$ip = $_SERVER['REMOTE_ADDR'];
$file = 'ratelimits.txt';
if (!file_exists($file)) {
    touch($file);
}
$lines = file($file, FILE_IGNORE_NEW_LINES | FILE_SKIP_EMPTY_LINES);
$limit = 10;  
$timeout = 60;

foreach ($lines as $line) {
    list($old_ip, $old_time) = explode(',', $line);
    if ($old_ip == $ip && time() - $old_time < $timeout) {
        echo json_encode(['success' => false, 'message' => 'Rate limit exceeded']);
        return;
    }
}

file_put_contents($file, "$ip," . time() . "\n", FILE_APPEND | LOCK_EX);

if ($_SERVER['REQUEST_METHOD'] === 'POST') { 
    try {
        $stmt = null;
        if ($_GET['url'] === "register") { 
            $stmt = $db->prepare('INSERT INTO users (username, password) VALUES (:username, :password)');
            $stmt->execute([':username' => $username, ':password' => password_hash($password, PASSWORD_DEFAULT)]);
            echo json_encode(['success' => true, 'message' => 'Registration successful']);
        } else if ($_GET['url'] === "login") { 
            $stmt = $db->prepare('SELECT password FROM users WHERE username = :username');
            $stmt->execute([':username' => $username]);
            $row = $stmt->fetch(PDO::FETCH_ASSOC);
            if ($row && password_verify($password, $row['password'])) {
                echo json_encode(['success' => true, 'message' => 'Login successful']);
            } else {
                echo json_encode(['success' => false, 'message' => 'Invalid username or password']);
            }
        } else {
            echo json_encode(['success' => false, 'message' => 'Invalid request']);
        }
    } catch (PDOException $ex) {
        error_log($ex->getMessage());
        echo json_encode(['success' => false, 'message' => 'An error occurred']);
    }
}
?>