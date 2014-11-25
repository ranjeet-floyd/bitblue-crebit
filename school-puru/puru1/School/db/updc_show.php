<div class="main-container">
	<div class="post-header">
		<span>Add Students to Class</span>
	</div>
	<div class="post-content">
<?php
parse_str($_POST['sch_updc'],$arr);
include('../../db.php');
$result=mysqli_query($con,"SELECT * from sch_class where medium='".$arr['sch_updc_show_mdm']."' AND std='".$arr['sch_updc_show_std']."' ");
//echo("SELECT * from sch_class where medium='".$arr['sch_updc_show_mdm']."' AND std='".$arr['sch_updc_show_std']."'");

//echo (mysqli_num_rows($result));
if(mysqli_num_rows($result)!=0){
    $query= mysqli_query($con,"SELECT * from user_sch where medium='".$arr['sch_updc_show_mdm']."' AND std='".$arr['sch_updc_show_std']."' AND Section='".$arr['sch_updc_show_div']."' group by section, name");
	$i=0;
	if(mysqli_num_rows($query)!=0){
	while($set[$i]=mysqli_fetch_row($query)){					
		$i++;
	}
	$set=array_filter($set);	
	$box=0;
	//print_r($set);
	foreach($set as $key=>$val){
	//echo $key;
		$box++;
		if($key==0){
			$div=$val[4];
			?><div class="post-text check-box-class <?php echo 'update-'.$div ?>"><span style="display:block;font-weight:bold">
			Section: <?php echo $div; ?>
			</span>
				<div class="check-box box-left">
				 <input  type="checkbox" class="check-select-all" value="<?php echo 'update-'.$div ?>">
					<label class="checkbox-text">Select All</label> 
						<span class="check-box-span">
				 <?php
		}
		else{
		?>
		<?php
			if($val[4]!=$div){
			
			?>
			</span>
			</div>
			
			<div class="box-right" style="width:200px; display:inline:margin-top:20px;">
			Medium:<select name="update-mdm">
							<option value="default">Select one</option>
							 <option value="English">English</option>
							 <option value="Hindi">Hindi</option>
							 <option value="Marathi">Marathi</option>
					</select><br/>
				Standard:<select name="update-to">
				<option value="default">Select one</option>
				    <option value="Mr.dextro">Mr.dextro</option>
							 <option value="nursery">Nursery</option>
                             <option value="junior.kg">jr.kg</option>
                             <option value="senior.kg">sr.kg</option>
							 <option value="first">First</option>
							 <option value="second">Second</option>
							 <option value="third">Third</option>
							 <option value="fourth">Fourth</option>
							 <option value="fifth">Fifth</option>
							 <option value="sixth">Sixth</option>
							 <option value="seventh">Seventh</option>
							 <option value="eighth">Eighth</option>
							 <option value="ninth">Ninth</option>
							 <option value="tenth">Tenth</option>
							 
				
				</select>
				<br/>
				Section:<select name="update-sec">
							<option value="default">Select one</option>
							
					</select><br/>
				<button class= "updc_class" type="submit" id= <?php echo 'update-'.$div; ?> ><span>Update</span></button> 
			<?php
				$div=$val[4];
				$box=1;
				?></div></div>
				<div class=" post-text check-box-class <?php echo "update-".$div ?> ">
					<span style="display:block;font-weight:bold;">
						Section: <?php echo $div; ?>
					</span>
				<div class="check-box box-left">
	
			
				<input type="checkbox"  class="check-select-all" value="<?php echo 'update-'.$div ?>" >
				<label class="checkbox-text">Select All</label>	
				<span class="check-box-span">
				<?php

			}
		}
		?>
							 <div class="box-check-left" >
							 <input type="checkbox" value="<?php echo $val[5] ?>"name="<?php echo 'update-'.$div ?>"> 
								<span class="box-check-span">
									<?php print_r($val[5]);?>
								</span>
								<span class="box-check-span" style="width:70px;"> 
									<?php print_r($val[1]);?>
								</span>			  					  
							 </div>
		<?php
	}
		?>
				
		<?php
		//echo $box;
		

?></span> 
			</div>
			<div class="box-right" style="width:200px;display:inline margin-top:20px;">
			Medium:<select name="update-mdm">
								<option value="default">Select one</option>
							 <option value="English">English</option>
							 <option value="Hindi">Hindi</option>
							 <option value="Marathi">Marathi</option>
						</select><br/>
				Standard:<select name="update-to" class="update-to">
				<option value="default">Select one</option>
				   <option value="Mr.dextro">Mr.dextro</option>
							 <option value="nursery">Nursery</option>
                             <option value="junior.kg">jr.kg</option>
                             <option value="senior.kg">sr.kg</option>
							 <option value="first">First</option>
							 <option value="second">Second</option>
							 <option value="third">Third</option>
							 <option value="fourth">Fourth</option>
							 <option value="fifth">Fifth</option>
							 <option value="sixth">Sixth</option>
							 <option value="seventh">Seventh</option>
							 <option value="eighth">Eighth</option>
							 <option value="ninth">Ninth</option>
							 <option value="tenth">Tenth</option>
				</select>
			<br/>
			Section:<select name="update-sec">
							<option value="default">Select one</option>
							
					</select><br/>
			<button class= "updc_class" type="submit" id=<?php echo 'update-'.$div; ?> ><span>Update</span></button></div>
<?php
	}
else{
?>
<span>No Student in this class</span>
<?php
}
?>
</div></div>
<?php
}	
else{
?>
<span>No Class found to Search result</span>
<?php
}
?>
</div>
</div>
<?php
exit;
?>
