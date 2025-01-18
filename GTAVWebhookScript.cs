// دالة لإظهار المركبة أمام اللاعب بمقدار 15 وحدة
private void SpawnVehicleInFront()
{
    // حساب الموقع أمام اللاعب بمقدار 15 وحدة
    Vector3 spawnPosition = Game.Player.Character.Position + Game.Player.Character.ForwardVector * 15f;
    
    // تحديد نوع المركبة التي سيتم استدعاؤها (هنا اخترنا "Adder" كمثال)
    VehicleHash vehicleHash = VehicleHash.Adder;  // يمكنك تغيير المركبة حسب رغبتك
    
    // إنشاء المركبة في المكان المحدد أمام اللاعب
    Vehicle vehicle = World.CreateVehicle(new Model(vehicleHash), spawnPosition);
    
    // طباعة رسالة في السجل
    Logger.Log("Vehicle spawned in front of player at position: " + spawnPosition.ToString());
}

private void OnKeyDown(object sender, KeyEventArgs e)
{
    // تحقق إذا كان الزر المضغوط هو 'H'
    if (e.KeyCode == Keys.H)
    {
        // استدعاء الدالة لإنشاء المركبة أمام اللاعب
        SpawnVehicleInFront();
    }
}
